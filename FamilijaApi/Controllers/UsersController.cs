using Microsoft.AspNetCore.Mvc;
using FailijaApi.Data;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using FamilijaApi.DTOs;
using FamilijaApi.Models;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UsersController : ControllerBase
    {
        private IUserRepo _userRepo;
        private IMapper _mapper;

        public UsersController(IUserRepo userRepo, IMapper mapper)
        {
            _mapper = mapper;
            _userRepo=userRepo;
        }

        public async Task<IActionResult> GeAlltUsers()
        {
            var items=await _userRepo.GetAllItems();
            if(items==null)     return NoContent();
            var userlist = new List<User>();

            foreach (var item in items)
            {
                userlist.Add(await _userRepo.GetUserById(item.ReferralId));
            }

            var mapedItems = _mapper.Map<List<UserReadDto>>(items);

            for (int i = 0; i < mapedItems.Count; i++)
            {
                mapedItems[i].ReferralUser = userlist[i];
            }
            
            return Ok(mapedItems);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            var response=await _userRepo.GetUserById(id);
                if(response==null)      return NoContent();
            return Ok(_mapper.Map<UserReadDto>(response));
        }

        [HttpPost]

        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);
            _userRepo.CreateUser(user);
            await _userRepo.SaveChanges();

            return Created("", user);


        }
    }
}