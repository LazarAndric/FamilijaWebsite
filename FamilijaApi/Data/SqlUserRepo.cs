using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private FamilijaDbContext _context;
        public SqlUserRepo(FamilijaDbContext context){
            _context= context;
        }
        public async Task<IEnumerable<User>> GetAllItems()
        {
            return await _context.User.ToArrayAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.User.FirstOrDefaultAsync(item=> item.Id==id);
        }
    }
}