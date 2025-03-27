using Microsoft.EntityFrameworkCore;
using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class LiveReportRepo : ILiveReportRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public LiveReportRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LiveReport> GetAsync(Guid id)
        {
            return await _dbContext.SubmitLiveReports.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(LiveReport submitLiveReport)
        {
            await _dbContext.AddAsync(submitLiveReport);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(LiveReport submitLiveReport)
        {
            submitLiveReport.LastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _dbContext.Update(submitLiveReport);
            await _dbContext.SaveChangesAsync();
        }
    }
}
