using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IRoleRepo
    {
        Task<IEnumerable<User>> GetAllItems();
        Task<User> GetUserById(int id);
    }
}