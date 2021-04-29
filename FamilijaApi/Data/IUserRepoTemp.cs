using System.Threading.Tasks;
using FamilijaApi.Models;

namespace FamilijaApi.Data
{
    public interface IUserRepoTemp
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CheckPassword(User existingUser, string password);
        Task CreateUserAsync(User user);
    }
}