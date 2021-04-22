using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace FamilijaApi.Data
{
    public class SqlAddressRepo : IAdddressesRepo
    {
        private FamilijaContext _context;
        public SqlAddressRepo(FamilijaContext context){
            _context= context;
        }
    }
}