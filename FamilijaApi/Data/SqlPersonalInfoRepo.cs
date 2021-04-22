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
        private FamilijaContext _context;
        public SqlPersonalInfoRepo(FamilijaContext context){
            _context= context;
        }
    }
}