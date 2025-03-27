using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public interface ILiveReportRepo
    {
        Task AddAsync(LiveReport submitLiveReport);
        Task<LiveReport> GetAsync(Guid id);
        Task UpdateAsync(LiveReport submitLiveReport);
    }
}