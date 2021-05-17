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

        public async Task<RefreshToken> GetTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token.Equals(token));
        }
        public async Task<RefreshToken> GetTokenByuserIdAsync(int id)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id);
        }
        public async Task UpdateTokenAsync(int id, RefreshToken token)
        {
            _context.Entry(await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == id)).CurrentValues.SetValues(token);
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