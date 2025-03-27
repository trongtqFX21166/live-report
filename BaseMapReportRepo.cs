using Microsoft.EntityFrameworkCore;
using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.BuildingBlocks.Parameters;
using VietmapLive.TrafficReport.Core.Data;
using VietmapLive.TrafficReport.Core.Entities;
using VietmapLive.TrafficReport.Core.Repositories;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class BasemapReportRepo : IBasemapReportRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public BasemapReportRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BasemapReport> GetAsync(Guid id)
        {
            return await _dbContext.BasemapReports.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == id);
        }

        public async Task AddAsync(BasemapReport baseMapReport)
        {
            await _dbContext.AddAsync(baseMapReport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(BasemapReport baseMapReport)
        {
            baseMapReport.LastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _dbContext.Update(baseMapReport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PaginatedResult<UserHistoryReportDto>> GetUserHistoryReportsAsync(int pageIndex, int pageSize, int userId)
        {
            var response = new PaginatedResult<UserHistoryReportDto>();

            var basemapReports = _dbContext.BasemapReports.Where(s => s.UserId == userId).Select(t => new UserHistoryReportDto {
                ReportId = t.Id, 
                CategoryId = t.CategoryId,
                SubmitTime = t.SubmitTime,
                Status = t.Status,
                NumberOfView = t.NumberOfView,
                NumberOfConfirm = t.NumberOfConfirm,
                Type = ReportType.BasemapReport,
                StreetViewImageUrl = t.StreetViewImageUrl,
                Direction = t.Direction,
                Lat = t.Lat,
                Lng = t.Lng,
                Address = t.Address,
                UserLat = t.UserLat,
                UserLng = t.UserLng,
                UserAddress = t.UserAddress
            });

            var submitLiveReports = _dbContext.SubmitLiveReports.Where(s => s.UserId == userId).Select(t => new UserHistoryReportDto
            {
                ReportId = t.Id,
                CategoryId = t.CategoryId,
                SubmitTime = t.SubmitTime,
                Status = t.Status,
                NumberOfView = t.NumberOfView,
                NumberOfConfirm = t.NumberOfConfirm,
                Type = ReportType.LiveReport,
                StreetViewImageUrl = null,
                Direction = null,
                Lat = t.Lat,
                Lng = t.Lng,
                Address = t.Address,
                UserLat = t.UserLat,
                UserLng = t.UserLng,
                UserAddress = t.UserAddress
            });

            var combinedQuery = basemapReports.Union(submitLiveReports).OrderByDescending(s => s.SubmitTime);

            response.TotalRecords = await combinedQuery.CountAsync();
            response.TotalPages = (int)Math.Ceiling((double)response.TotalRecords / pageSize);

            response.Results = await combinedQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return response;
        }

        public async Task<int> GetTotalReportsAsync(int userId, long? fromTime, long? toTime)
        {
            var basemapQuery = _dbContext.BasemapReports.Where(s => s.UserId == userId);
            var liveQuery = _dbContext.SubmitLiveReports.Where(s => s.UserId == userId);

            if(fromTime.HasValue)
            {
                basemapQuery = basemapQuery.Where(s => s.SubmitTime >= fromTime);
                liveQuery = liveQuery.Where(s => s.SubmitTime >= fromTime);
            }

            if (toTime.HasValue)
            {
                basemapQuery = basemapQuery.Where(s => s.SubmitTime <= toTime);
                liveQuery = liveQuery.Where(s => s.SubmitTime <= toTime);
            }

            int total = await basemapQuery.CountAsync();
            total += await liveQuery.CountAsync();
           
            return total;
        }
    }
}
