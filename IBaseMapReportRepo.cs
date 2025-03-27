using VietmapLive.TrafficReport.Core.Data;
using VietmapLive.TrafficReport.Core.Entities;
using VietmapLive.BuildingBlocks.CommonResponse;

namespace VietmapLive.TrafficReport.Core.Repositories
{
    public interface IBasemapReportRepo
    {
        Task AddAsync(BasemapReport baseMapReport);
        Task<BasemapReport> GetAsync(Guid id);
        Task<int> GetTotalReportsAsync(int userId, long? fromTime, long? toTime);
        Task<PaginatedResult<UserHistoryReportDto>> GetUserHistoryReportsAsync(int pageIndex, int pageSize, int userId);
        Task UpdateAsync(BasemapReport baseMapReport);
    }
}
