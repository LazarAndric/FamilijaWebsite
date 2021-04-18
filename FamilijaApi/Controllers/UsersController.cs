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
using FamilijaApi.DTOs;
using AutoMapper;
using AutoMapper.Configuration;

namespace FamilijaApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserRepo _userRepo;
        private IMapper _mapper;

        public UserController(IUserRepo userRepo, IMapper mapper)
        {
            _mapper = mapper;
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
        public async Task<ActionResult<UserReadDTO>> GetById(int id){
            var response=await _userRepo.GetUserById(id);
            if(response==null)
                return NoContent();
            return Ok(_mapper.Map<UserReadDTO>(response));
        }
    }
}