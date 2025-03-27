using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public interface IReportConfigRepo
    {
        Task<ReportConfig> GetAsync(string code);
    }
}