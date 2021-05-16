using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Net.Mail;
using System.Security.Cryptography;

namespace FamilijaApi.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private FamilijaDbContext _context;
        public SqlUserRepo(FamilijaDbContext context){
            _context= context;
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(x=>MailAddress.Equals(x.EMail, email));
        }
        
        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
            
        }

        public void DeleteUser(User deleteModelUser)
        {
            _context.Users.Remove(deleteModelUser);
        }

        public async Task<IEnumerable<User>> GetAllItems()
        {
            return await _context.Users.ToArrayAsync();
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(item=> item.Id==id);
        }


        public async Task<bool> SaveChangesAsync()
        {
           return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateUser(User updateModelUser)
        {

        }

        public void UpdateCUser(bool v)
        {

        }

        public async Task<User> FindReferalAsync(string code)
        {
            return await _context.Users.FirstOrDefaultAsync(item => item.ReferralCode == code);
        }

        public Task FindReferalAsync(int referalID)
        {
            throw new NotImplementedException();
        }

        //public Task FindReferalAsync(int referalID)
        //{
        //    return _context.Users.FirstOrDefaultAsync(x => User.Equals(x.ReferralId, x.DateRegistration, referalID));
        //}

    }
}