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
        public async Task CreateRole(UserRole role)
        {
            await _context.UserRoles.AddAsync(role);
        }

        public void DeleteRole(Role deleteModelRole)
        {
            if (deleteModelRole == null)
            {
                throw new ArgumentNullException(nameof(deleteModelRole));
            }
            _context.Roles.Remove(deleteModelRole);
        }
        public async Task<UserRole> GetRole(int id)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(item => item.UserId == id);
        }
        public async Task<Role> GetRoleByRoleId(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(item => item.Id == id);
        }
        public async Task<Role> GetRoleByRoleNamed(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(item => item.Value == name);
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