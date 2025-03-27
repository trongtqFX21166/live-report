using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.TrafficReport.Core.Data;
using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Core.Repositories
{
    public interface ITrafficCategoryRepo
    {
        Task AddAsync(TrafficCategory trafficCategory);
        Task<TrafficCategory> GetAsync(Guid id);
        Task<TrafficCategory> GetAsync(string name);
        Task<List<TrafficCategory>> GetSubCategoriesAsync(Guid parentId);
        Task<List<TrafficCategory>> GetTrafficCategoriesAsync();
        Task<PaginatedResult<TrafficCategory>> SearchAsync(SearchTrafficReportRequest request);
        Task UpdateAsync(TrafficCategory trafficCategory);
        Task UpdateRangeAsync(List<TrafficCategory> trafficCategories);
    }
}