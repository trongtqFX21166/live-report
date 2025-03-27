using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VietmapLive.TrafficReport.Core.Entities.Vml;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories.Vml
{
    public class VmlReadDbContext : DbContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;

        public VmlReadDbContext(IConfiguration config,
            DbContextOptions<VmlReadDbContext> options)
            : base()
        {
            _config = config;
            _connectionString = _config["ConnectionStrings:VmlReadDb"] ?? String.Empty;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseNpgsql(_connectionString);
            }
        }

        public virtual DbSet<User> Users => Set<User>();
    }
}
