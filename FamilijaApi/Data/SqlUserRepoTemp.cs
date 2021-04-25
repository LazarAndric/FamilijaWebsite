using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FamilijaApi.Controllers;
using System.Net.Mail;

namespace FamilijaApi.Data
{
    public class SqlUserRepoTemp : IUserRepoTemp
    {
        private FamilijaDbContext _context;
        public SqlUserRepoTemp(FamilijaDbContext context){
            _context= context;
        }

        public bool CheckPassword(User existingUser, string password)
        {
            var isTrue =string.Equals(existingUser.Password, password);
            return isTrue;

        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(x=>MailAddress.Equals(x.EMail, email));
        }
        public async Task CreateUser(User user)
        {
           await _context.Users.AddAsync(user);
        }
    }
}