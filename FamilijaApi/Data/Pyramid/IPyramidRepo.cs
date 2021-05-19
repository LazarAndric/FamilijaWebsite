using FamilijaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Data.Pyramid
{
    public interface IPyramidRepo
    {
         IEnumerable<Finance> GetFinanceAsync(int id);
         Task<bool> SaveChangesAsync();
        Task<bool> CreateUserTvAsync(UserTv usertv);
        IEnumerable<UserTv> GetUserTvAsync(int id);
        void DeleteUserTv(UserTv deliteModelUserTv);
    }
}
