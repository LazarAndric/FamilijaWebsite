using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FamilijaApi.Data
{
    public class SqlUserRepo : IUserRepo
    {
        private FamilijaDbContext _context;
        public SqlUserRepo(FamilijaDbContext context){
            _context= context;
        }

        public async void CreateUser(User user)
        {
           await _context.Users.AddAsync(user);
        }

        public void DeleteUser(User deleteModelUser)
        {
            if (deleteModelUser == null)
            {
                throw new ArgumentNullException(nameof(deleteModelUser));
            }
            _context.Users.Remove(deleteModelUser);
        }

        public async Task<IEnumerable<User>> GetAllItems()
        {
            return await _context.Users.ToArrayAsync();
        }

        public async Task<User> GetPassword(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(item=> item.Id==id);
        }

        public async Task<bool> SaveChanges()
        {
           return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateUser(User updateModelUser)
        {

        }

    }
}