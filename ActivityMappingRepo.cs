using Microsoft.EntityFrameworkCore;
using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class ActivityMappingRepo : IActivityMappingRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public ActivityMappingRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ActivityMapping> GetAsync(Guid categoryId)
        {
            return await _dbContext.ActivityMappings.FirstOrDefaultAsync(s => s.CategoryId == categoryId);
        }
    }
}
