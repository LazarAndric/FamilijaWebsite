using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FamilijaApi.Data
{
    public class SqlPasswordRepo : IPasswordRepo
    {
        private FamilijaDbContext _context;

        public SqlPasswordRepo(FamilijaDbContext context)
        {
            _context = context;
        }

        public async Task CreatePasswordAsync(Password password)
        {
            await _context.Passwords.AddAsync(password);
        }

        public void DeletePassword(Password deleteModelPassword)
        {
            if (deleteModelPassword == null)
            {
                throw new ArgumentNullException(nameof(deleteModelPassword));
            }
            _context.Passwords.Remove(deleteModelPassword);
        }

        public async Task<Password> GetPasswordAsync(int userId)
        {
            return await _context.Passwords.FirstOrDefaultAsync(item => item.UserId == userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdatePassword(Password updateModelPassword)
        {

        }
    }
}
