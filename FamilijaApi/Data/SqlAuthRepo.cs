using System.Linq;
using System.Threading.Tasks;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FamilijaApi.Data
{
    public class SqlAuthRepo : IAuthRepo
    {
        private FamilijaDbContext _context;
        public SqlAuthRepo(FamilijaDbContext context)
        {
            _context=context;
        }

        public async Task AddToDbAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);

        }

        public async Task<RefreshToken> GetToken(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token.Equals(token));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void DeleteToken(RefreshToken storedToken)
        {
             _context.RefreshTokens.Remove(storedToken);
        }
    }
}