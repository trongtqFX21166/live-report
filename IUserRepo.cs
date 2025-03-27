using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietmapLive.TrafficReport.Core.Entities.Vml;

namespace VietmapLive.TrafficReport.Core.Repositories
{
    public interface IUserRepo
    {
        Task<User> GetAsync(int id);
    }
}
