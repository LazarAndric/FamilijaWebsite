using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FamilijaApi.Data
{
    public class SqlRoleRepo : IRoleRepo
    {
        private FamilijaDbContext _context;
        public SqlRoleRepo(FamilijaDbContext context){
            _context = context;
        }

        public async void CreateRole(Role role)
        {
            await _context.Roles.AddAsync(role);
        }

        public void DeleteRole(Role deleteModelRole)
        {
            if (deleteModelRole == null)
            {
                throw new ArgumentNullException(nameof(deleteModelRole));
            }
            _context.Roles.Remove(deleteModelRole);
        }

        public async Task<Role> GetRole(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateRole(Role updateModelRole)
        {

        }
    }
}