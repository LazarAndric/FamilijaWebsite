using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlPersonalInfoRepo : IPersonalInfoRepo
    {
        private FamilijaDbContext _context;
        public SqlPersonalInfoRepo(FamilijaDbContext context){
            _context= context;
        }

        public async void CreatePersonalInfo(PersonalInfo personalInfo)
        {
            await _context.PersonalInfo.AddAsync(personalInfo);
        }

        public async Task<PersonalInfo> GetPersonalInfo(int id)
        {
            return await _context.PersonalInfo.FirstOrDefaultAsync(item => item.UserId == id);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}