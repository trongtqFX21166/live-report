using Microsoft.EntityFrameworkCore;
using VietmapLive.TrafficReport.Core.Entities.Vml;
using VietmapLive.TrafficReport.Core.Repositories;

namespace VietmapLive.TrafficReport.Infrastructure.Repositories.Vml
{
    public class UserRepo : IUserRepo
    {
        private readonly VmlReadDbContext _dbContext;

        public UserRepo(VmlReadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}
