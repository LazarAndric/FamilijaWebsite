using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlRoleRepo : IRoleRepo
    {
        private FamilijaDbContext _context;
        public SqlRoleRepo(FamilijaDbContext context){
            _context= context;
        }

        public async Task<Role> GetRole(int id)
        {
            return await _context.Role.FirstOrDefaultAsync(role=> role.Id==id);
        }
    }
}