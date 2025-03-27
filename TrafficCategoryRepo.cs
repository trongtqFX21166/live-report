using Microsoft.EntityFrameworkCore;
using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.BuildingBlocks.Extensions;
using VietmapLive.TrafficReport.Core.Data;
using VietmapLive.TrafficReport.Core.Entities;
using VietmapLive.TrafficReport.Core.Repositories;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories
{
    public class TrafficCategoryRepo : ITrafficCategoryRepo
    {
        private readonly TrafficReportDbContext _dbContext;

        public TrafficCategoryRepo(TrafficReportDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TrafficCategory trafficCategory)
        {
            trafficCategory.LastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var orderCategory = await _dbContext.TrafficCategories.OrderByDescending(s => s.Order).FirstOrDefaultAsync();

            if (orderCategory != null)
            {
                trafficCategory.Order = trafficCategory.Order + 1;
            }

            await _dbContext.AddAsync(trafficCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRangeAsync(List<TrafficCategory> trafficCategories)
        {
            _dbContext.UpdateRange(trafficCategories);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrafficCategory trafficCategory)
        {
            trafficCategory.LastModified = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            _dbContext.Update(trafficCategory);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TrafficCategory>> GetTrafficCategoriesAsync()
        {
            return await _dbContext.TrafficCategories.Where(s => !s.IsDeleted && s.IsActive).ToListAsync();
        }

        public async Task<TrafficCategory> GetAsync(string name)
        {
            return await _dbContext.TrafficCategories.FirstOrDefaultAsync(s => !s.IsDeleted && s.Name == name);
        }

        public async Task<TrafficCategory> GetAsync(Guid id)
        {
            return await _dbContext.TrafficCategories.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == id);
        }

        public async Task<List<TrafficCategory>> GetSubCategoriesAsync(Guid parentId)
        {
            return await _dbContext.TrafficCategories.Where(s => !s.IsDeleted && s.ParentId == parentId).ToListAsync();
        }

        public async Task<PaginatedResult<TrafficCategory>> SearchAsync(SearchTrafficReportRequest request)
        {
            var response = new PaginatedResult<TrafficCategory>();

            var query = _dbContext.TrafficCategories.Where(s => !s.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(s => EF.Functions.Like(EF.Functions.Unaccent(s.Name), $"%{request.SearchText.UnaccentAndLower()}%"));
            }

            if (!string.IsNullOrEmpty(request.ReportType))
            {
                query = query.Where(s => s.Type == request.ReportType);
            }

            if (request.IsActive.HasValue)
            {
                query = query.Where(s => s.IsActive == request.IsActive.Value);
            }

            if (request.IsDisplayOnMap.HasValue)
            {
                query = query.Where(s => s.IsDisplayOnMap == request.IsDisplayOnMap.Value);
            }

            var total = await query.CountAsync();

            response.TotalRecords = await query.CountAsync();
            response.TotalPages = (int)Math.Ceiling((double)response.TotalRecords / request.PageSize);

            response.Results = await query
                .OrderBy(s => s.Order)
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return response;
        }
    }
}
