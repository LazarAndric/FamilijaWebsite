using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlRoleRepo : IUserRepo
    {
        private FamilijaContext _context;
        public SqlRoleRepo(FamilijaContext context){
            _context= context;
        }
        public async Task<IEnumerable<User>> GetAllItems()
        {
            return await _context.Users.ToArrayAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(item=> item.Id==id);
        }
    }
}