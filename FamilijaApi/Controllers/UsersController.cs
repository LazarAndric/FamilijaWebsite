using Microsoft.AspNetCore.Mvc;
using FamilijaApi.Data;
using System.Threading.Tasks;
using FamilijaApi.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using FailijaApi.Data;

namespace FamilijaApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepo _userRepo;
        
        public UserController(IUserRepo userRepo)
        {
            _userRepo=userRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(){

            var response=await _userRepo.GetAllItems();
            if(response==null)
                return NoContent();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<User>>> GetById(int id){

            var response=await _userRepo.GetUserById(id);
            if(response==null)
                return NoContent();
            return Ok(response);
        }
    }
}