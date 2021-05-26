using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FamilijaApi.Data
{
    public class SqlAddressRepo : IAdddressesRepo
    {
        private FamilijaDbContext _context;
        public SqlAddressRepo(FamilijaDbContext context){
            _context= context;
        }

        public async void CreateAddress(Address addres)
        {
            await _context.Addresses.AddAsync(addres);
        }

        public void DeleteAdress(Address deleteModelAddress)
        {
            if (deleteModelAddress == null)
            {
                throw new ArgumentNullException(nameof(deleteModelAddress));
            }
            _context.Addresses.Remove(deleteModelAddress);
        }

        public async Task<Address> GetAddressAsync(int userId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(item => item.UserId == userId);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateAddress(Address updateModelAddress)
        {

        }
    }
}