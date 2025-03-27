using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.Common.Models;
using VietmapLive.TrafficReport.Api.Models.Dtos;
using VietmapLive.TrafficReport.Api.Models.Request;
using VietmapLive.TrafficReport.Core.Data;

namespace VietmapLive.TrafficReport.Api.Services
{
    public interface ITrafficReportService
    {
        Task<ApiResponse<ItemsResponse<CategoryDto>>> GetAllCategoriesAsync();
        Task<ApiResponse<BaseApiOutputCommon>> SubmitBasemapReportAsync(SubmitBasemapReportRequest request);

        Task<ApiResponse<BaseApiOutputCommon>> SubmitLiveReportAsync(SubmitLiveReportRequest request);
        Task<ApiResponse<BaseApiOutputCommon>> EditTrafficReportAsync(EditTrafficReportRequest request);
        Task<ApiResponse<PaginatedResult<UserHistoryReportDto>>> GetUserHistoryReportsAsync(int pageIndex, int pageSize, int userId);
        Task<int> GetTotalReportsAsync(int userId, long? fromTime, long? toTime);
    }
}