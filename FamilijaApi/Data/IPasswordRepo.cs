using FamilijaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Data
{
    public interface IPasswordRepo
    {
        Task<Password> GetPasswordAsync(int id);
        Task CreatePasswordAsync(Password password);
        Task<bool> SaveChangesAsync();
        void UpdatePassword(Password updateModelPassword);
        void DeletePassword(Password deleteModelPassword);
    }
}
