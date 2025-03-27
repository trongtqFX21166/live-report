using Microsoft.EntityFrameworkCore;
using VietmapLive.TrafficReport.Core.Entities;
using VietmapLive.TrafficReport.Core.Repositories;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class BasemapReportImageRepo : IBasemapReportImageRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public BasemapReportImageRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<BasemapReportImage>> GetImagesAsync(Guid reportId)
        {
            return await _dbContext.BasemapReportImages.Where(s => s.ReportId == reportId).ToListAsync();
        }

        public async Task AddRangeAsync(List<BasemapReportImage> basemapReportImages)
        {
            await _dbContext.AddRangeAsync(basemapReportImages);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(List<BasemapReportImage> basemapReportImages)
        {
            _dbContext.RemoveRange(basemapReportImages);
            await _dbContext.SaveChangesAsync();
        }
    }
}
