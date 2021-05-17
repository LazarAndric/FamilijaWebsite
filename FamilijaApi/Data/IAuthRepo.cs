using System.Threading.Tasks;
using FamilijaApi.Models;

namespace FamilijaApi.Data
{
    public interface IAuthRepo
    {
        Task AddToDbAsync(RefreshToken refreshToken);
        Task SaveChangesAsync();
        Task<RefreshToken> GetTokenAsync(string token);
        Task UpdateTokenAsync(string jti, RefreshToken token);
        void DeleteToken(RefreshToken storedToken);
        Task<RefreshToken> GetTokenByJtiAsync(string id);
    }
}