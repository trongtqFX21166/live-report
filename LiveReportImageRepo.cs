using VietmapLive.TrafficReport.Core.Entities;
using VietmapLive.TrafficReport.Core.Repositories;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class LiveReportImageRepo : ILiveReportImageRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public LiveReportImageRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(List<LiveReportImage> liveReportImages)
        {
            await _dbContext.AddRangeAsync(liveReportImages);
            await _dbContext.SaveChangesAsync();
        }
    }
}
