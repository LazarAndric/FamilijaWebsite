using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlRoleRepo : IRoleRepo
    {
        private FamilijaContext _context;
        public SqlRoleRepo(FamilijaContext context){
            _context= context;
        }
    }
}