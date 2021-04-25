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
           
            return Ok(_mapper.Map<List<UserReadDto>>(items));

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

        [HttpPut ("{id}")]

        public async Task<IActionResult> UpdateUser(int id, UserUpdateDto userUpdateDto)
        {
            var updateModelUser = _userRepo.GetUserById(id).Result;
            if (updateModelUser == null)
            {
                return NotFound();
            }

            _mapper.Map(userUpdateDto, updateModelUser);
            _userRepo.UpdateUser(updateModelUser);
            await _userRepo.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var deleteModelUser = _userRepo.GetUserById(id).Result;
            if (deleteModelUser == null)
            {
                return NotFound();
            }

            _userRepo.DeleteUser(deleteModelUser);
            _userRepo.SaveChanges();
            return NoContent();
        }


    }
}