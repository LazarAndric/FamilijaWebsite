using FamilijaApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Data
{
    public interface IPasswordRepo
    {
        Task<Password> GetPassword(int id);
        Task CreatePassword(Password password);
        Task<bool> SaveChanges();
        void UpdatePassword(Password updateModelPassword);
        void DeletePassword(Password deleteModelPassword);
    }
}
