using Microsoft.AspNetCore.Mvc;
using FamilijaApi.Data;
using System.Threading.Tasks;
using FamilijaApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace FamilijaApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly FamilijaContext _context;
        
        public UserController(FamilijaContext context, ILogger<UserController> logger)
        {
            _logger=logger;
            _context=context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(){

            var response= await _context.Users.ToArrayAsync();
            if(response==null)
                return NoContent();
            return Ok(response);
        }
    }
}