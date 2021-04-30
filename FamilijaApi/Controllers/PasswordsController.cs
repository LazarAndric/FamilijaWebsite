using AutoMapper;
using FamilijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.Models;
using FamilijaApi.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FamilijaApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PasswordsController : ControllerBase
    {
        private IPasswordRepo _passwordRepo;
        private IMapper _mapper;

        public PasswordsController(IPasswordRepo passwordRepo, IMapper mapper)
        {
            _passwordRepo = passwordRepo;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PasswordReadDto>> GetPassword(int id)
        {
            var content = await _passwordRepo.GetPassword(id);
            if (content == null) return NoContent();
            return Ok(_mapper.Map<PasswordReadDto>(content));
        }

        


        [HttpPost]
        public async Task<IActionResult> CreatePassword([FromBody] PasswordCreateDto passwordCreateDto)
        {
            var password = _mapper.Map<Password>(passwordCreateDto);
            string password1 = passwordCreateDto.Password;
            Password hash = PasswordUtility.GenerateSaltedHash(10, password1);
            hash.UserId = password.UserId;
            await _passwordRepo.CreatePassword(hash);

            await _passwordRepo.SaveChanges();

            return Created("", hash);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassword(int id, PasswordUpdateDto passwordUpdateDto)
        {
            var updateModelPassword = _passwordRepo.GetPassword(id).Result;
            if (updateModelPassword == null)
            {
                return NotFound();
            }
            string password1 = updateModelPassword.Hash;
            

            Password hash = PasswordUtility.GenerateSaltedHash(10, password1);
            hash.UserId = passwordUpdateDto.UserId;
            _mapper.Map(passwordUpdateDto, hash);
            _passwordRepo.UpdatePassword(hash);

            await _passwordRepo.SaveChanges();
            return Ok();

        }

        [HttpDelete("{id}")]
        public ActionResult DeleteAddress(int id)
        {
            var deleteModelPassword = _passwordRepo.GetPassword(id).Result;
            if (deleteModelPassword == null)
            {
                return NotFound();
            }

            _passwordRepo.DeletePassword(deleteModelPassword);
            _passwordRepo.SaveChanges();
            return NoContent();
        }

    }
}
