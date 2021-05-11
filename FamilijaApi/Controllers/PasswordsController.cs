using AutoMapper;
using FamilijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Models;
using FamilijaApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PasswordsController : ControllerBase
    {
        private IPasswordRepo _passwordRepo;
        private IMapper _mapper;

        public PasswordsController(IPasswordRepo passwordRepo, IMapper mapper)
        {
            _passwordRepo = passwordRepo;
            _mapper = mapper;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PasswordReadDto>> ChangePassword(int id /*FROM TOKEN*/ ,[FromBody] PasswordChangeRequest passwords)
        {
            if(!PasswordUtility.ValidatePassword(passwords.NewPassword, out string msg))
            {
                BadRequest(JwtTokenUtility.Result(false, msg));
            }
            if(!passwords.NewPassword.Equals(passwords.ConfirmPassword))
            {
                BadRequest(JwtTokenUtility.Result(false, "Your confirm password is not same"));
            }
            var currentHashedPass=await _passwordRepo.GetPassword(id);
            var currentPass= PasswordUtility.VerifyPassword(passwords.CurrentPassword, currentHashedPass.Hash, currentHashedPass.Salt);
            if(!currentPass){
                BadRequest(JwtTokenUtility.Result(false, "Your password is not correct"));
            }
            var newPass=PasswordUtility.GenerateSaltedHash(10, passwords.NewPassword);
            newPass.Id=id;
            try
            {
                await _passwordRepo.CreatePassword(newPass);
                await _passwordRepo.SaveChanges();
                return Created("", JwtTokenUtility.Result(true, "User is created"));
            }
            catch (System.Exception)
            {
                return BadRequest(JwtTokenUtility.Result(false,"Can't create password"));
            }
        }


    }
}
