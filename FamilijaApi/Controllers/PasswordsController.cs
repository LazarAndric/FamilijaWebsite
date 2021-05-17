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
        public async Task<ActionResult<PasswordReadDto>> ChangePassword([FromHeader] string authorize, PasswordChangeRequest passwordModel)
        {
            try
            {
                var auth=await _jwtTokenUtil.VerifyJwtToken(authorize);
                if(auth.Success){
                    if(!PasswordUtility.ValidatePassword(passwordModel.NewPassword, out string msg))
                    {
                        throw new System.Exception();
                    }
                    if(!passwordModel.NewPassword.Equals(passwordModel.ConfirmPassword))
                    {
                        throw new System.Exception();
                    }
                    var currentHashedPass=await _passwordRepo.GetPasswordAsync(auth.User.Id);
                    var currentPass= PasswordUtility.VerifyPassword(passwordModel.CurrentPassword, currentHashedPass.Hash, currentHashedPass.Salt);
                    if(!currentPass){
                        throw new System.Exception();
                    }
                    var newPass=PasswordUtility.GenerateSaltedHash(10, passwordModel.NewPassword);
                    newPass.Id=auth.User.Id;
                    try
                    {
                        await _passwordRepo.CreatePasswordAsync(newPass);
                        await _passwordRepo.SaveChangesAsync();
                        throw new System.Exception();
                    }
                    catch (System.Exception)
                    {
                        throw new System.Exception();
                    }
                }
                throw new System.Exception();
            }
            catch (System.Exception)
            {
                return Unauthorized();
            }
        }


    }
}
