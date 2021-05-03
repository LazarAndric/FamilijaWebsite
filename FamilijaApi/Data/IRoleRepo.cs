using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IRoleRepo
    {
        Task<UserRole> GetRole(int id);
        Task<Role> GetRoleByRoleId(int id);
        Task CreateRole(UserRole role);
        Task<bool> SaveChanges();
        void UpdateRole(Role updateModelRole);
        void DeleteRole(Role deleteModelRole);
        Task<Role> GetRoleByRoleNamed(string name);
    }
}