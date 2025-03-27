using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VietmapLive.TrafficReport.Core.Repositories;
using VietmapLive.TrafficReport.Infrastructure.Repositories;
using VietmapLive.TrafficReport.Infrastructure.Repositories.Vml;

namespace VietmapLive.TrafficReport.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<TrafficReportDbContext>(option => option.UseNpgsql(configuration.GetConnectionString("Database")));

            services.AddDbContextPool<VmlReadDbContext>(option => option.UseNpgsql(configuration.GetConnectionString("VmlReadDb")));

            services.AddTransient<IBasemapReportRepo, BasemapReportRepo>();
            services.AddTransient<ILiveReportRepo, LiveReportRepo>();
            services.AddTransient<ITrafficCategoryRepo, TrafficCategoryRepo>();
            services.AddTransient<ILiveReportImageRepo, LiveReportImageRepo>();
            services.AddTransient<IBasemapReportImageRepo, BasemapReportImageRepo>();
            services.AddTransient<IReportConfigRepo, ReportConfigRepo>();
            services.AddTransient<ITrafficCategoryAuditLogRepo, TrafficCategoryAuditLogRepo>();
            services.AddTransient<IUserRepo, UserRepo>();
            services.AddTransient<IActivityMappingRepo, ActivityMappingRepo>();

            return services;
        }
    }
}
