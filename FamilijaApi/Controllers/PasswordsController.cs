using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.Configuration;
using FamilijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Models;
using FamilijaApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        private readonly JwtTokenUtility _jwtTokenUtil;

        public PasswordsController(IRoleRepo roleRepo, IUserRepo userRepo, IAuthRepo authRepo, IPasswordRepo passwordRepo, IMapper mapper, IOptionsMonitor<Jwtconfig> optionsMonitor, TokenValidationParameters tokenValidation)
        {
            _passwordRepo = passwordRepo;
            _mapper = mapper;
            _jwtTokenUtil=new JwtTokenUtility(authRepo, userRepo, roleRepo, optionsMonitor.CurrentValue, tokenValidation);
        }

        [HttpPut]
        public async Task<ActionResult<PasswordReadDto>> ChangePassword([FromBody] GetCommunicationModel<PasswordChangeRequest> data)
        {
            var auth=await _jwtTokenUtil.VerifyAndGenerateToken(data.TokenRequest);
            var finalAuth= _mapper.Map<AuthResult>(auth);
            if(auth.Success){
                if(!PasswordUtility.ValidatePassword(data.GenericModel.NewPassword, out string msg))
                {
                    BadRequest(JwtTokenUtility.ResultPW(false, msg));
                }
                if(!data.GenericModel.NewPassword.Equals(data.GenericModel.ConfirmPassword))
                {
                    BadRequest(JwtTokenUtility.ResultPW(false, "Your confirm password is not same"));
                }
                var currentHashedPass=await _passwordRepo.GetPassword(auth.Id);
                var currentPass= PasswordUtility.VerifyPassword(data.GenericModel.CurrentPassword, currentHashedPass.Hash, currentHashedPass.Salt);
                if(!currentPass){
                    BadRequest(JwtTokenUtility.ResultPW(false, "Your password is not correct"));
                }
                var newPass=PasswordUtility.GenerateSaltedHash(10, data.GenericModel.NewPassword);
                newPass.Id=auth.Id;
                try
                {
                    await _passwordRepo.CreatePassword(newPass);
                    await _passwordRepo.SaveChanges();
                    return Created("", JwtTokenUtility.ResultPW(true, "User is created"));
                }
                catch (System.Exception)
                {
                    return BadRequest(JwtTokenUtility.ResultPW(false,"Can't create password"));
                }
            }
            return Unauthorized(finalAuth);
        }


    }
}
