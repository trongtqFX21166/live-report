using Microsoft.AspNetCore.Mvc;
using Platform.Serilog;
using VietmapLive.TrafficReport.Api.Services;
using VietmapLive.TrafficReport.Infrastructure;
using VietmapCloud.Shared.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using VietmapCloud.Shared.RestrictAccessSwaggerPage;
using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapCloud.Shared.WebCoreBase.Middleware;
using IdentityServer4.AccessTokenValidation;
using VietmapCloud.Shared.Redis;
using System.Linq;
using VietmapLive.FtpServer;
using Microsoft.Extensions.Options;
using System.Configuration;
using Elastic.Apm.Api;
using Platform.KafkaClient.Models;
using Platform.KafkaClient;

namespace VietmapLive.TrafficReport.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));


            builder.Services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(option =>
                {
                    option.Authority = builder.Configuration["IdentityServer:Authority"];
                    option.RequireHttpsMetadata = false;
                    option.ApiName = builder.Configuration["IdentityServer:ApiName"];
                    option.ApiSecret = builder.Configuration["IdentityServer:ApiSecret"];
                });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


            builder.Services.AddTransient<ITrafficReportService, TrafficReportService>();

            builder.Services.Configure<FtpServerConfig>(builder.Configuration.GetSection("FtpServer"));

            builder.Services.AddSingleton<IFtpServer>(sp =>
            {
                var settings = sp.GetRequiredService<IOptionsMonitor<FtpServerConfig>>().CurrentValue;
                return new FtpServer.FtpServer(settings);
            });

            builder.Services.Configure<ProducerConfig>("Producer", builder.Configuration.GetSection("Producer"));
            builder.Services.AddSingleton<IProducer>(s =>
            {
                var settings = s.GetRequiredService<IOptionsMonitor<ProducerConfig>>().Get("Producer");
                var logger = s.GetRequiredService<ILogger<Producer>>();

                return new Producer(logger, settings);
            });

            builder.Services.AddInfrastructureServices(builder.Configuration);

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddCors();

            builder.Host.RegisterSerilogConfig();

            builder.Services.AddSwaggerWithDefaultValues();

            builder.Services.AddApiVersioning(option =>
            {
                option.DefaultApiVersion = new ApiVersion(1, 0);
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.ReportApiVersions = true;
                //option.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });

            builder.Services.AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVV";
                option.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddApiVersioning(config =>
            {
                config.ErrorResponses = new CustomErrorApiVersionResponse();
            });

            var app = builder.Build();

            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseGlobalExceptionHandler();

            // Configure the HTTP request pipeline.
            //app.UseMiddleware<FilterAccessSwaggerPageMiddleware>();
            app.UseSwaggerSupportMultipleVersion(app.Services.GetService<IApiVersionDescriptionProvider>());

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}