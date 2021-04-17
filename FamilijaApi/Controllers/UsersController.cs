using Microsoft.AspNetCore.Mvc;
using FamilijaApi.Data;
using System.Threading.Tasks;
using FamilijaApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace FamilijaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    class UsersController : ControllerBase
    {
        private readonly FamilijaContext _context;
        
        public UsersController(FamilijaContext context)
        {
            _context=context;
        }

        [HttpGet]
        public ActionResult<User> GetUsers(){
            Console.WriteLine("Done");Console.ReadLine();
            var respone1=string.Empty;
            var response= _context.Users.ToList();
            return Ok(response);
        }
    }
}