using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Core.Repositories
{
    public interface IBasemapReportImageRepo
    {
        Task AddRangeAsync(List<BasemapReportImage> basemapReportImages);
        Task<List<BasemapReportImage>> GetImagesAsync(Guid reportId);
        Task RemoveRangeAsync(List<BasemapReportImage> basemapReportImages);
    }
}