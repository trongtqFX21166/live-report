using Microsoft.EntityFrameworkCore;
using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class ReportConfigRepo : IReportConfigRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public ReportConfigRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ReportConfig> GetAsync(string code)
        {
            return await _dbContext.ReportConfigs.FirstOrDefaultAsync(s => !s.IsDeleted && s.Code == code);
        }
    }
}
