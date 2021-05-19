using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Data.Pyramid
{
    public class SqlPyramidRepo : IPyramidRepo
    {
        private FamilijaDbContext _context;

        public SqlPyramidRepo(FamilijaDbContext context)
        {
            _context = context;
        }

        public  IEnumerable<Finance> GetFinanceAsync(int id)
        {
            return  _context.Finance.Where(x => x.UserId == id).ToArray();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }


        public async Task<bool> CreateUserTvAsync(UserTv usertv)
        {
            try
            {
                await _context.UserTvs.AddAsync(usertv);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }
                
        public IEnumerable<UserTv> GetUserTvAsync(int id)
        {
            return _context.UserTvs.Where(x => x.UserId == id).ToArray();
        }

        public void DeleteUserTv(UserTv deliteModelUserTv)
        {
            _context.UserTvs.Remove((UserTv)deliteModelUserTv);
        }
    }
}
