using Microsoft.AspNetCore.Mvc;
using FailijaApi.Data;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using FamilijaApi.DTOs;
using FamilijaApi.Models;
using System;
using System.Security.Cryptography;
using System.Net.Mail;
using FamilijaApi.Utility;
using FamilijaApi.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;

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
            _userRepo = userRepo;
        }

        public async Task<IActionResult> GeAlltUsers()
        {
            var items=await _userRepo.GetAllItems();
            if(items==null)     return NoContent();
            var userlist = new List<User>();
           
            return Ok(_mapper.Map<List<UserReadDto>>(items));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _userRepo.GetUserByIdAsync(id);
            if (response == null) return NoContent();
            return Ok(_mapper.Map<UserReadDto>(response));
        }

        [HttpPost]

        public async Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);
            await _userRepo.CreateUserAsync(user);
            await _userRepo.SaveChanges();

            return Created("", user);
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UserUpdateDto userUpdateDto)
        {
            var updateModelUser = _userRepo.GetUserByIdAsync(id).Result;
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
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            var deleteModelUser = await _userRepo.GetUserByIdAsync(id);
            if (deleteModelUser == null)
            {
                return NotFound();
            }
            _userRepo.DeleteUser(deleteModelUser);
            await _userRepo.SaveChanges();
            return NoContent();
        }

        [HttpGet("{action}")]

        public async Task<IActionResult> SendMail([FromBody]Email mail)
        {
            
           await MailUtility.SendEmail(mail);
             return Ok();
        }

        [HttpGet("{action}/{id}/{link}/{body}")]

        public async Task<IActionResult> SendConfirmedMail(int id, string link, string body)
        {
            var mail = new Email();
            mail.To = new List<string>();
            var e = await _userRepo.GetUserByIdAsync(id);
            string email = e.EMail.ToString();
            mail.To.Add(email);
            mail.Subject = "Confirmed Mail";
            string b = " <a href='"+link+"'> Click here </a>";

            mail.Body = b;
            await MailUtility.SendEmail(mail);
            return Ok();
        }

        [HttpPut("{action}/{id}")]
        public async Task<IActionResult> UpdateConfirmedMail(int id, ConfirmedEmailDto confirmedEmailDto)
        {
            var updateModelUser = _userRepo.GetUserByIdAsync(id).Result;
            if (updateModelUser == null)
            {
                return NotFound();
            }

            if (updateModelUser.EmailConfirmed == true)
            {
                return Ok("Mejl je verifikovan");
            }

            _mapper.Map(confirmedEmailDto, updateModelUser);
            _userRepo.UpdateCUser(updateModelUser.EmailConfirmed = true);
            await _userRepo.SaveChanges();
            return Ok("Uspesno ste verifikovali mejl");
        }

        
    }
}