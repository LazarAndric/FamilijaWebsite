using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IUserRepo
    {
        Task<User> FindByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllItems();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> CreateUserAsync(User user);
        Task<bool> SaveChanges();
        void UpdateUser(User updateModelUser);
        void DeleteUser(User deleteModelUser);
    }
}