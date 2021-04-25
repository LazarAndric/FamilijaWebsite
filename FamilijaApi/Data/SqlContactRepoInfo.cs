using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlContactRepo : IContactInfoRepo
    {
        private FamilijaDbContext _context;
        public SqlContactRepo(FamilijaDbContext context){
            _context= context;
        }
    }
}