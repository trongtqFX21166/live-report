using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Core.Repositories
{
    public interface ILiveReportImageRepo
    {
        Task AddRangeAsync(List<LiveReportImage> liveReportImages);
    }
}