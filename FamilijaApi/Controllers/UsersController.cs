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
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepo _userRepo;
        private IMapper _mapper;

        public UsersController(IUserRepo userRepo, IMapper mapper)
        {
            _mapper = mapper;
            _userRepo=userRepo;
        }

        public async Task<ActionResult<List<UserReadDto>>> GeAlltUsers()
        {
            var items=await _userRepo.GetAllItems();
            if(items==null)     return NoContent();
            return Ok(_mapper.Map<List<UserReadDto>>(items));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserReadDto>> GetById(int id){
            var response=await _userRepo.GetUserById(id);
                if(response==null)      return NoContent();
            return Ok(_mapper.Map<UserReadDto>(response));
        }
    }
}