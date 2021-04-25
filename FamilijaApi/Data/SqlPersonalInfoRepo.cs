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
    }
}