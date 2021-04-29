using AutoMapper;
using FamilijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.Models;
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

        public static Password GenerateSaltedHash(int size, string password)
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            Password hashSalt = new Password { Hash = hashPassword, Salt = salt };
            return hashSalt;
        }

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }



        [HttpPost]
        public async Task<IActionResult> CreatePassword([FromBody] PasswordCreateDto passwordCreateDto)
        {
            var password = _mapper.Map<Password>(passwordCreateDto);
            string password1 = password.Hash;
            Password hash = GenerateSaltedHash(10, password1);
            hash.UserId = password.UserId;
            _passwordRepo.CreatePassword(hash);

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
            

            Password hash = GenerateSaltedHash(10, password1);
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
