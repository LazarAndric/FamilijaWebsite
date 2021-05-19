using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Data
{
    public class SqlFinanceRepo : IFinanceRepo
    {
        private FamilijaDbContext _context;

        public SqlFinanceRepo(FamilijaDbContext context)
        {
            _context = context;
        }

        public async void CreateFinance(Finance finance)
        {
            await _context.Finance.AddAsync(finance);
        }

        public void DeleteUser(Finance deleteModelFinance)
        {
            if (deleteModelFinance == null)
            {
                throw new ArgumentNullException(nameof(deleteModelFinance));
            }
            _context.Finance.Remove(deleteModelFinance);
        }

        public async Task<Finance> GetFinance(int id)
        {
            return await _context.Finance.FirstOrDefaultAsync(item => item.UserId == id);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }



        public void UpdateFinance(Finance updateModelFinance)
        {

        }
    }
}
