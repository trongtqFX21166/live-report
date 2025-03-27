using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.TrafficReport.Core.Data;
using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Core.Repositories
{
    public interface ITrafficCategoryAuditLogRepo
    {
        Task AddAsync(TrafficCategoryAuditLog trafficCategoryAuditLog);
        Task AddRangeAsync(List<TrafficCategoryAuditLog> trafficCategoryAuditLogs);
        Task<PaginatedResult<TrafficCategoryAuditLogDto>> SearchAsync(SearchTrafficCategorySnapshotsRequest request);
    }
}
