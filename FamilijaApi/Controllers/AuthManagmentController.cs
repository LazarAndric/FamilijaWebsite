using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FailijaApi.Data;
using FamilijaApi.Configuration;
using FamilijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.DTOs.Response;
using FamilijaApi.Models;
using FamilijaApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FamilijaApi.Controllers
{
    
    [Route("auth")]
    [ApiController]
    [Authorize(AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme)]
    public class AuthManagmentController : ControllerBase
    {
        private AuthResult _result;
        private readonly IRoleRepo _rolerepo;
        private readonly IMapper _mapper;
        private readonly IAuthRepo _authRepo;
        private readonly IUserRepo _userRepo;
        private readonly IPasswordRepo _passwordRepo;
        private readonly Jwtconfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidation;

        public AuthManagmentController(IPasswordRepo passwordRepo, IRoleRepo roleRepo, TokenValidationParameters tokenValidation, IMapper mapper, IAuthRepo authRepo, IUserRepo userRepo, IOptionsMonitor<Jwtconfig> optionsMonitor)
        {
            _passwordRepo=passwordRepo;
            _rolerepo=roleRepo;
            _tokenValidation=tokenValidation;
            _mapper = mapper;
            _authRepo =authRepo;
            _userRepo=userRepo;
            _jwtConfig=optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user){
            if(ModelState.IsValid){
                var existingUser=new User();
                if(MailAddress.TryCreate(user.Username, out var mail))
                    existingUser= await _userRepo.FindByEmailAsync(mail.Address);
                else    existingUser= await _userRepo.GetUserByUsernameAsync(user.Username);
                if(existingUser==null)
                {
                    _result=JwtTokenUtility.Result(false,"Invalid username");
                    return BadRequest(_result);
                }

                var pass= await _passwordRepo.GetPassword(existingUser.Id);
                var isCorrect= PasswordUtility.VerifyPassword(user.Password, pass.Hash, pass.Salt);
                
                if(!isCorrect)
                {
                    _result=JwtTokenUtility.Result(false,"Invalid password");
                    return BadRequest(_result);
                }

                var existRole= await _rolerepo.GetRole(existingUser.Id);
                if(existingUser==null){
                    _result=JwtTokenUtility.Result(false,"Invalid login request");
                    return BadRequest(_result);
                }
                var role= await _rolerepo.GetRoleByRoleId(existingUser.Id);
                
                var jwtToken= await JwtTokenUtility.GenerateJwtToken(existingUser, role, _jwtConfig, _authRepo);

                return Ok(new CommunicationModel<User>(){
                        AuthResult=jwtToken,
                        GenericModel= existingUser
                });
            }
            var result=JwtTokenUtility.Result(false,"Invalid payload");
            return BadRequest(result);
        }

        [HttpPost]
        [Route("Register")]
        [Authorize(Roles="MODERATOR")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if(ModelState.IsValid)
            {
                var existing = await _userRepo.FindByEmailAsync(user.Email);
                if(existing!=null){
                    _result=JwtTokenUtility.Result(false,"Email already in use");
                    return BadRequest(_result);
                }
                existing = await _userRepo.GetUserByUsernameAsync(user.Username);
                if(existing!=null){
                    _result=JwtTokenUtility.Result(false,"Username already in use");
                    return BadRequest(_result);
                }
                var newUser = new User(){ EMail= user.Email, Username= user.Username};
                var isCreated= await _userRepo.CreateUserAsync(newUser);
                UserRole role= new UserRole(){RoleId=1,UserId=2};
                await _rolerepo.CreateRole(role);
                var existRole=await _rolerepo.GetRoleByRoleId(role.RoleId);
                var jwtToken=await JwtTokenUtility.GenerateJwtToken(newUser,existRole,_jwtConfig, _authRepo);
                return Ok(new CommunicationModel<User>(){
                    AuthResult=jwtToken,
                    GenericModel= newUser
                });
            }

            _result=JwtTokenUtility.Result(false,"Invalid payload");
            return BadRequest(_result);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest){
            if(ModelState.IsValid)
            {
                _result = await JwtTokenUtility.VerifyAndGenerateToken(tokenRequest, _tokenValidation, _authRepo, _userRepo, _rolerepo, _jwtConfig);

                if(_result == null)
                {
                    _result=JwtTokenUtility.Result(false,"Invalid payload");
                    return BadRequest(_result);
                }
                return Ok(_result);
            }
            _result=JwtTokenUtility.Result(false,"Invalid payload");
            return BadRequest(new RegistrationResponse(){
               Errors=  new List<string>(){
                   "Invalid payload"
               },
               Success=false
            });
        }
    }
}