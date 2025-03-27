using AutoMapper;
using Newtonsoft.Json;
using Platform.KafkaClient;
using System.Drawing.Printing;
using VietmapLive.BuildingBlocks.Common;
using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.BuildingBlocks.Parameters;
using VietmapLive.Common.Models;
using VietmapLive.FtpServer;
using VietmapLive.TrafficReport.Api.Models.Dtos;
using VietmapLive.TrafficReport.Api.Models.Request;
using VietmapLive.TrafficReport.Core.Data;
using VietmapLive.TrafficReport.Core.Entities;
using VietmapLive.TrafficReport.Core.Repositories;
using VietmapLive.TrafficReport.Infrastructure.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VietmapLive.TrafficReport.Api.Services
{
    public class TrafficReportService : ITrafficReportService
    {
        private readonly IBasemapReportRepo _basemapReportRepo;
        private readonly ILiveReportRepo _submitLiveReportRepo;
        private readonly ITrafficCategoryRepo _trafficCategoryRepo;
        private readonly IMapper _mapper;
        private readonly IFtpServer _ftpServer;
        private readonly IConfiguration _configuration;
        private readonly ILiveReportImageRepo _liveReportImageRepo;
        private readonly IBasemapReportImageRepo _basemapReportImageRepo;
        private readonly IProducer _producer;
        private readonly string _event = "SubmittedReport";
        private readonly string _eventName = "Gửi báo cáo";

        public TrafficReportService(IBasemapReportRepo basemapReportRepo,
            IMapper mapper,
            ILiveReportRepo submitLiveReportRepo,
            ITrafficCategoryRepo trafficCategoryRepo,
            IFtpServer ftpServer,
            IConfiguration configuration,
            ILiveReportImageRepo liveReportImageRepo,
            IBasemapReportImageRepo basemapReportImageRepo,
            IProducer producer)
        {
            _basemapReportRepo = basemapReportRepo;
            _mapper = mapper;
            _submitLiveReportRepo = submitLiveReportRepo;
            _trafficCategoryRepo = trafficCategoryRepo;
            _ftpServer = ftpServer;
            _configuration = configuration;
            _liveReportImageRepo = liveReportImageRepo;
            _basemapReportImageRepo = basemapReportImageRepo;
            _producer = producer;
        }

        private List<string> GetPaths(List<BasemapReportImage> images)
        {
            var paths = new List<string>();

            images.ForEach(s =>
            {
                if (!string.IsNullOrEmpty(s.ImageUrl))
                {
                    paths.Add(s.ImageUrl.Substring(_configuration["FtpServer:Uri"].Length));
                }
            });

            return paths;
        }

        private async Task ClearImageAsync(Guid reportId, List<string>? images)
        {
            var imagesDelete = await _basemapReportImageRepo.GetImagesAsync(reportId);

            if (imagesDelete?.Count > 0)
            {
                if(images?.Count > 0)
                {
                    var imagesUrl = images.Where(s => s.StartsWith("http"));

                    imagesDelete = imagesDelete.Where(s => !imagesUrl.Contains(s.ImageUrl)).ToList();
                }

                var paths = GetPaths(imagesDelete);
                _ftpServer.DeleteFile(paths);

                await _basemapReportImageRepo.RemoveRangeAsync(imagesDelete);
            }
        }

        public async Task<ApiResponse<BaseApiOutputCommon>> EditTrafficReportAsync(EditTrafficReportRequest request)
        {
            var response = new BaseApiOutputCommon();
            response.ReturnSuccess();

            var directions = new List<string>()
            {
                "SameDirection",
                "OppositeDirection"
            };

            if (!directions.Contains(request.Direction))
            {
                response.ReturnFail("Field Direction invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            if (request.Images?.Count() > 0)
            {
                var validateImage = ValidateImage(request.Images);

                if (validateImage.ResponseCode != BaseResponseCode.Success)
                {
                    response.ResponseCode = validateImage.ResponseCode;
                    response.Description = validateImage.Description;

                    return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
                }
            }

            var trafficCategory = await _trafficCategoryRepo.GetAsync(request.CategoryId);

            if (trafficCategory == null)
            {
                response.ReturnObjectDoesNotExist("Field Category invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            if (trafficCategory.Type != ReportType.BasemapReport)
            {
                response.ReturnFail("Field Category invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            var report = await _basemapReportRepo.GetAsync(request.ReportId);

            if (report == null)
            {
                response.ReturnObjectDoesNotExist("ReportId not found.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            report.CategoryId = request.CategoryId;
            report.Direction = request.Direction;

            await _basemapReportRepo.UpdateAsync(report);

            await ClearImageAsync(report.Id, request.Images);

            if (request.Images?.Count > 0)
            {
                var imageUrls = UploadImage(request.Images.Where(s => !s.StartsWith("http")).ToList());

                await SaveBasemapReportImagesAsync(report.Id, imageUrls);
            }

            return ResponseOutputCreaterCommon.SuccessResponse(response);
        }

        private float Filesize(string base64String)
        {
            byte[] bytes = System.Convert.FromBase64String(base64String);
            return (bytes.Length / 1024f) / 1024f;
        }

        public BaseApiOutputCommon ValidateImage(List<string> images)
        {
            var result = new BaseApiOutputCommon();
            result.ReturnSuccess();

            var extensions = new List<string>() { "IVBOR", "/9J/4" };

            float totalSize = 0;

            foreach (var image in images)
            {
                bool isUrl = FileCommon.CheckUrlValid(image);

                if (!isUrl)
                {
                    var validateResult = FileCommon.IsValidExtensionFromBase64String(extensions, image);

                    if (!validateResult)
                    {
                        result.ReturnFileExtensionIsNotValid();

                        return result;
                    }

                    totalSize += Filesize(image);
                }
            }

            if (totalSize > 4)
            {
                result.ReturnInValidFilesize($"Giới hạn size hình là {4} mb.");

                return result;
            }

            return result;
        }

        private List<string> UploadImage(List<string> images)
        {
            List<string> data = null;

            if (images?.Count > 0)
            {
                data = new List<string>();

                foreach (var item in images)
                {
                    var directory = $"production/vietmap-live/images/traffic-report/{DateTime.Now.Date.ToString(FormatStringParameter.yyyy_MM_dd)}";
                    var fileName = FileCommon.GetFileName(item);

                    _ftpServer.Upload(directory, fileName, new MemoryStream(Convert.FromBase64String(item)));

                    string imageUrl = $"{_configuration["FtpServer:Uri"]}{directory}/{fileName}";

                    data.Add(imageUrl);
                }
            }

            return data;
        }

        public async Task SaveLiveReportImagesAsync(Guid reportId, List<string> images)
        {
            var liveReportImages = new List<LiveReportImage>();

            int count = 1;

            foreach (var image in images)
            {
                liveReportImages.Add(new LiveReportImage
                {
                    ReportId = reportId,
                    ImageUrl = image,
                    Order = count,
                });

                count++;
            }

            await _liveReportImageRepo.AddRangeAsync(liveReportImages);
        }

        public async Task SaveBasemapReportImagesAsync(Guid reportId, List<string> images)
        {
            if(images == null || images.Count == 0)
            {
                return; 
            }

            var basemapReportImages = new List<BasemapReportImage>();

            int count = 1;

            foreach (var image in images)
            {
                basemapReportImages.Add(new BasemapReportImage
                {
                    ReportId = reportId,
                    ImageUrl = image,
                    Order = count,
                });

                count++;
            }

            await _basemapReportImageRepo.AddRangeAsync(basemapReportImages);
        }

        public async Task<ApiResponse<BaseApiOutputCommon>> SubmitBasemapReportAsync(SubmitBasemapReportRequest request)
        {
            var directions = new List<string>()
            {
                "SameDirection",
                "OppositeDirection"
            };

            var response = new BaseApiOutputCommon();
            response.ReturnSuccess();

            if (!directions.Contains(request.Direction))
            {
                response.ReturnFail("Field Direction invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            if (request.Images?.Count() > 0)
            {
                var validateImage = ValidateImage(request.Images);

                if (validateImage.ResponseCode != BaseResponseCode.Success)
                {
                    response.ResponseCode = validateImage.ResponseCode;
                    response.Description = validateImage.Description;

                    return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
                }
            }

            var trafficCategory = await _trafficCategoryRepo.GetAsync(request.CategoryId);

            if (trafficCategory == null)
            {
                response.ReturnObjectDoesNotExist("Field Category invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            if (trafficCategory.Type != ReportType.BasemapReport)
            {
                response.ReturnFail("Field Category invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            var entity = _mapper.Map<BasemapReport>(request);
            entity.Id = Guid.NewGuid();
            entity.Status = BaseMapReportStatus.WaitingVerification;

            await _basemapReportRepo.AddAsync(entity);

            if (request.Images?.Count > 0)
            {
                var imageUrls = UploadImage(request.Images);

                await SaveBasemapReportImagesAsync(entity.Id, imageUrls);
            }
           
            return ResponseOutputCreaterCommon.SuccessResponse(response);
        }


        public async Task<ApiResponse<BaseApiOutputCommon>> SubmitLiveReportAsync(SubmitLiveReportRequest request)
        {
            var response = new BaseApiOutputCommon();
            response.ReturnSuccess();

            if (request.Images?.Count() > 0)
            {
                var validateImage = ValidateImage(request.Images);

                if (validateImage.ResponseCode != BaseResponseCode.Success)
                {
                    response.ResponseCode = validateImage.ResponseCode;
                    response.Description = validateImage.Description;

                    return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
                }
            }

            var trafficCategory = await _trafficCategoryRepo.GetAsync(request.CategoryId);

            if (trafficCategory == null)
            {
                response.ReturnObjectDoesNotExist("Field Category invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            if (trafficCategory.Type != ReportType.LiveReport)
            {
                response.ReturnFail("Field Category invalid.");
                return ResponseOutputCreaterCommon.ErrorResponse<BaseApiOutputCommon>(response);
            }

            var entity = _mapper.Map<LiveReport>(request);
            entity.Id = Guid.NewGuid();
            entity.Status = BaseMapReportStatus.WaitingVerification;

            await _submitLiveReportRepo.AddAsync(entity);

            if (request.Images?.Count > 0)
            {
                var imageUrls = UploadImage(request.Images);

                await SaveLiveReportImagesAsync(entity.Id, imageUrls);
            }

            await _producer.ProduceMessageAsync(JsonConvert.SerializeObject(new VMLTrafficReportEventDto()
            {
                Id = entity.Id,
                UserId = entity.UserId, 
                CategoryId = entity.CategoryId, 
                SubmitTime = entity.SubmitTime,
                CreatedAt = entity.CreatedAt,
                Event = _event,
                EventName = _eventName
            }));

            return ResponseOutputCreaterCommon.SuccessResponse(response);
        }

        public async Task<int> GetTotalReportsAsync(int userId, long? fromTime, long? toTime)
        {
            return await _basemapReportRepo.GetTotalReportsAsync(userId, fromTime, toTime);
        }

        public async Task<ApiResponse<PaginatedResult<UserHistoryReportDto>>> GetUserHistoryReportsAsync(int pageIndex, int pageSize, int userId)
        {
            var response = await _basemapReportRepo.GetUserHistoryReportsAsync(pageIndex, pageSize, userId);

            if (response.TotalRecords == 0)
            {
                return ResponseOutputCreaterCommon.SuccessResponse(response);
            }

            var categories = await _trafficCategoryRepo.GetTrafficCategoriesAsync();

            foreach (var item in response.Results)
            {
                var category = categories.FirstOrDefault(s => s.Id == item.CategoryId);

                if (category == null)
                {
                    continue;
                }

                item.IconUrl = category?.IconUrl;
                item.Name = category?.Name;
                item.MarkerImageUrl = category?.MarkerImageUrl;
                item.Type = category.Type;
                item.Images = null;
                item.ParentId = category.ParentId;

                if (item.Type == ReportType.BasemapReport)
                {
                    var images = await _basemapReportImageRepo.GetImagesAsync(item.ReportId);

                    if (images == null || images.Count == 0)
                    {
                        continue;
                    }

                    item.Images = images.Select(s => s.ImageUrl).ToList();
                }
            }

            return ResponseOutputCreaterCommon.SuccessResponse(response);
        }

        public async Task<ApiResponse<ItemsResponse<CategoryDto>>> GetAllCategoriesAsync()
        {
            var response = new ItemsResponse<CategoryDto>()
            {
                Results = new List<CategoryDto>()
            };

            var categories = await _trafficCategoryRepo.GetTrafficCategoriesAsync();

            if (categories == null || categories.Count == 0)
            {
                return ResponseOutputCreaterCommon.SuccessResponse(response);
            }

            response.Results = _mapper.Map<List<CategoryDto>>(categories);

            return ResponseOutputCreaterCommon.SuccessResponse(response);
        }

        //public async Task<ApiResponse<ItemsResponse<CategoryDto>>> GetAllCategoriesAsync()
        //{
        //    Guid guiddata = Guid.NewGuid();

        //    var response = new ItemsResponse<CategoryDto>()
        //    {
        //        Results = new List<CategoryDto>()
        //        {
        //            new CategoryDto { Id = Guid.NewGuid(), ParentId = null, Order = 1, Name = "Sai tốc độ", IconUrl = "https://api.vietmap.live/share/images/traffic-report/speed_violation.png", Type = "BaseMapReport", MinimizationHexColor = "#FF0000" },
        //    new CategoryDto { Id = Guid.NewGuid(), ParentId = null, Order = 2, Name = "Sai camera", IconUrl = "https://api.vietmap.live/share/images/traffic-report/wrong_camera.png", Type = "BaseMapReport", MinimizationHexColor = "#800080" },
        //    new CategoryDto { Id = guiddata, ParentId = null, Order = 3, Name = "Kẹt xe", IconUrl = "https://api.vietmap.live/share/images/traffic-report/traffic_jam.png", Type = "LiveReport", MinimizationHexColor = "#FFA500" },
        //    new CategoryDto { Id = Guid.NewGuid(), ParentId = null, Order = 4, Name = "Tai nạn", IconUrl = "https://api.vietmap.live/share/images/traffic-report/accident.png", Type = "LiveReport", MinimizationHexColor = "#008000" },
        //    new CategoryDto { Id = Guid.NewGuid(), ParentId = null, Order = 5, Name = "Nguy hiểm", IconUrl = "https://api.vietmap.live/share/images/traffic-report/danger.png", Type = "LiveReport", MinimizationHexColor = "#FFFF00" },
        //    new CategoryDto { Id = Guid.NewGuid(), ParentId = null, Order = 6, Name = "Đường ngập", IconUrl = "https://api.vietmap.live/share/images/traffic-report/flooded_road.png", Type = "LiveReport", MinimizationHexColor = "#0000FF" },

        //    new CategoryDto { Id = Guid.NewGuid(), ParentId = guiddata, Order = 7, Name = "Kẹt xe", IconUrl = "https://api.vietmap.live/share/images/traffic-report/traffic_jam.png", Type = "LiveReport", MinimizationHexColor = "#FFA500" },

        //    new CategoryDto { Id = Guid.NewGuid(), ParentId = guiddata, Order = 8, Name = "Kẹt xe nặng", IconUrl = "https://api.vietmap.live/share/images/traffic-report/heavy_traffic.png", Type = "LiveReport", MinimizationHexColor = "#0000FF" }
        //        }
        //    };

        //    return ResponseOutputCreaterCommon.SuccessResponse(response);
        //}
    }
}
