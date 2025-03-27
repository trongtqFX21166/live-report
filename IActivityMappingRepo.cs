using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public interface IActivityMappingRepo
    {
        Task<ActivityMapping> GetAsync(Guid categoryId);
    }
}