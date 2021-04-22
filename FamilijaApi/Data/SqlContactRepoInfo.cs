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
        private FamilijaContext _context;
        public SqlContactRepo(FamilijaContext context){
            _context= context;
        }
    }
}