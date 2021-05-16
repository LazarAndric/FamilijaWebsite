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
        Task<bool> CreateUserAsync(User user);
        Task<bool> SaveChangesAsync();
        void UpdateUser(User updateModelUser);
        void DeleteUser(User deleteModelUser);
        void UpdateCUser(bool v);
        Task<User> FindReferalAsync(string code);
        Task FindReferalAsync(int referalID);
    }
}