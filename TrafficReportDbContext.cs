using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class TrafficReportDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public TrafficReportDbContext(IConfiguration config,
            DbContextOptions<TrafficReportDbContext> options)
            : base()
        {
            _config = config;
            _connectionString = _config["ConnectionStrings:Database"] ?? String.Empty;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        public DbSet<BasemapReport> BasemapReports => Set<BasemapReport>();

        public DbSet<LiveReport> SubmitLiveReports => Set<LiveReport>();

        public DbSet<TrafficCategory> TrafficCategories => Set<TrafficCategory>();

        public DbSet<LiveReportImage> LiveReportImages => Set<LiveReportImage>();

        public DbSet<BasemapReportImage> BasemapReportImages => Set<BasemapReportImage>();

        public DbSet<ReportConfig> ReportConfigs => Set<ReportConfig>();

        public DbSet<TrafficCategoryAuditLog> TrafficCategoryAuditLogs => Set<TrafficCategoryAuditLog>();

        public DbSet<ActivityMapping> ActivityMappings => Set<ActivityMapping>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityMapping>(entity =>
            {
                entity.Property(e => e.ActivityReportName).HasMaxLength(50);
            });

            modelBuilder.Entity<ReportConfig>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(20);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<TrafficCategory>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.IconUrl).HasMaxLength(200);
                entity.Property(e => e.MarkerImageUrl).HasMaxLength(200);
                entity.Property(e => e.Type).HasMaxLength(20);
                entity.Property(e => e.DisplayTargetUser).HasMaxLength(20);
                entity.Property(e => e.MinimizationHexColor).HasMaxLength(7);
                entity.Property(e => e.DurationDisplayOnMapType).HasMaxLength(10);
                entity.Property(e => e.AddDurationToDisplayOnMapType).HasMaxLength(10);
                entity.Property(e => e.SubtractDurationToHideOnMapType).HasMaxLength(10);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);

                entity.HasMany(d => d.LiveReports)
                    .WithOne(p => p.TrafficCategory)
                    .HasForeignKey(d => d.CategoryId);

                entity.HasMany(d => d.BaseMapReports)
                    .WithOne(p => p.TrafficCategory)
                    .HasForeignKey(d => d.CategoryId);
            });

            modelBuilder.Entity<LiveReportImage>(entity =>
            {
                entity.Property(e => e.ImageUrl).HasMaxLength(200);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.LiveReport)
                    .WithMany(p => p.LiveReportImages)
                    .HasForeignKey(d => d.ReportId);
            });

            modelBuilder.Entity<TrafficCategoryAuditLog>(entity =>
            {
                entity.Property(e => e.Action).HasMaxLength(30);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<BasemapReportImage>(entity =>
            {
                entity.Property(e => e.ImageUrl).HasMaxLength(200);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.BaseMapReport)
                    .WithMany(p => p.BasemapReportImages)
                    .HasForeignKey(d => d.ReportId);
            });

            modelBuilder.Entity<LiveReport>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.CellId).HasMaxLength(100);
                entity.Property(e => e.RoadLinkId).HasMaxLength(100);
                entity.Property(e => e.UserAddress).HasMaxLength(500);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);
            });

            modelBuilder.Entity<BasemapReport>(entity =>
            {
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Reason).HasMaxLength(255);
                entity.Property(e => e.UserMapVer).HasMaxLength(20);
                entity.Property(e => e.Direction).HasMaxLength(20);
                entity.Property(e => e.Gps).HasMaxLength(255);
                entity.Property(e => e.VehicleType).HasMaxLength(20);
                entity.Property(e => e.RoadLinkId).HasMaxLength(100);
                entity.Property(e => e.CellId).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.UserAddress).HasMaxLength(500);
                entity.Property(e => e.CreatedBy).HasMaxLength(50);
                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);
            });
        }
    }
}
