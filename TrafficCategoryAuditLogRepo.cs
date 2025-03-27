using Microsoft.EntityFrameworkCore;
using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.BuildingBlocks.Extensions;
using VietmapLive.TrafficReport.Core.Data;
using VietmapLive.TrafficReport.Core.Entities;
using VietmapLive.TrafficReport.Core.Repositories;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class TrafficCategoryAuditLogRepo : ITrafficCategoryAuditLogRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public TrafficCategoryAuditLogRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PaginatedResult<TrafficCategoryAuditLogDto>> SearchAsync(SearchTrafficCategorySnapshotsRequest request)
        {
            var response = new PaginatedResult<TrafficCategoryAuditLogDto>();

                var query = from log in _dbContext.TrafficCategoryAuditLogs
                            join category in _dbContext.TrafficCategories
                            on log.CategoryId equals category.Id
                            select new TrafficCategoryAuditLogDto
                            {
                                UserId = log.UserId,
                                Action = log.Action,
                                ModifyDetails = log.ModifyDetails,
                                CategoryName = category.Name,
                                CreatedAt = log.CreatedAt
                            };

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(s => EF.Functions.Like(EF.Functions.Unaccent(s.CategoryName), $"%{request.SearchText.UnaccentAndLower()}%"));
            }

            if (request.FromTime.HasValue)
            {
                query = query.Where(s => s.CreatedAt >= request.FromTime.Value);
            }

            if (request.ToTime.HasValue)
            {
                query = query.Where(s => s.CreatedAt <= request.ToTime.Value);
            }

            
            var total = await query.CountAsync();

            response.TotalRecords = await query.CountAsync();
            response.TotalPages = (int)Math.Ceiling((double)response.TotalRecords / request.PageSize);

            response.Results = await query
                .OrderBy(s => s.CreatedAt)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return response;
        }

        public async Task AddAsync(TrafficCategoryAuditLog trafficCategoryAuditLog)
        {
            await _dbContext.AddAsync(trafficCategoryAuditLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task AddRangeAsync(List<TrafficCategoryAuditLog> trafficCategoryAuditLogs)
        {
            await _dbContext.AddRangeAsync(trafficCategoryAuditLogs);
            await _dbContext.SaveChangesAsync();
        }
    }
}
