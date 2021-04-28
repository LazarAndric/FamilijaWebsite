using System.Threading.Tasks;
using FamilijaApi.Models;

namespace FamilijaApi.Data
{
    public interface IAuthRepo
    {
        Task AddToDbAsync(RefreshToken refreshToken);
        Task SaveChangesAsync();
        Task<RefreshToken> GetToken(string token);
        void DeleteToken(RefreshToken storedToken);
    }
}