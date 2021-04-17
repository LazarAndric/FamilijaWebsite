using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilijaApi.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private FamilijaContext _context;
        public SqlUserRepo(FamilijaContext context){
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