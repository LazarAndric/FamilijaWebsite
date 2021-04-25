using System.Threading.Tasks;
using FamilijaApi.Models;

namespace FamilijaApi.Data
{
    public interface IUserRepoTemp
    {
        Task<User> FindByEmailAsync(string email);
        bool CheckPassword(User existingUser, string password);
        Task CreateUser(User user);
    }
}