using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlUserInfoRepo : IUserInfoRepo
    {
        private FamilijaContext _context;
        public SqlUserInfoRepo(FamilijaContext context){
            _context= context;
        }

        public async Task<UserInfo> GetUserInfo(int id)
        {
            
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.UserId==id);
        }
    }
}