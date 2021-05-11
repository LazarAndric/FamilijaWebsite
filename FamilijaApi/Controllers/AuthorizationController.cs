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

    [Route("[Controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes= JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorizationsController : ControllerBase
    {
        private AuthResult _result;
        private readonly IRoleRepo _rolerepo;
        private readonly IMapper _mapper;
        private readonly IAuthRepo _authRepo;
        private readonly IUserRepo _userRepo;
        private readonly IPasswordRepo _passwordRepo;
        private readonly Jwtconfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidation;

        public AuthorizationsController(IPasswordRepo passwordRepo, IRoleRepo roleRepo, TokenValidationParameters tokenValidation, IMapper mapper, IAuthRepo authRepo, IUserRepo userRepo, IOptionsMonitor<Jwtconfig> optionsMonitor)
        {
            _passwordRepo = passwordRepo;
            _rolerepo = roleRepo;
            _tokenValidation = tokenValidation;
            _mapper = mapper;
            _authRepo = authRepo;
            _userRepo = userRepo;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost("logIn")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user) {
            if (ModelState.IsValid) {
                var existingUser = new User();
                if (MailAddress.TryCreate(user.Username, out var mail))
                    existingUser = await _userRepo.FindByEmailAsync(mail.Address);
                if (existingUser == null)
                {
                    _result = JwtTokenUtility.Result(false, "Invalid username");
                    return BadRequest(_result);
                }

                var pass = await _passwordRepo.GetPassword(existingUser.Id);
                var isCorrect = PasswordUtility.VerifyPassword(user.Password, pass.Hash, pass.Salt);

                if (!isCorrect)
                {
                    _result = JwtTokenUtility.Result(false, "Invalid password");
                    return BadRequest(_result);
                }

                var existRole = await _rolerepo.GetRole(existingUser.Id);
                if (existingUser == null) {
                    _result = JwtTokenUtility.Result(false, "Invalid login request");
                    return BadRequest(_result);
                }
                var role = await _rolerepo.GetRoleByRoleId(existingUser.Id);

                var token = JwtTokenUtility.GenerateJwtToken(existingUser, role, _jwtConfig, _authRepo, out var jwtToken);

                await _authRepo.AddToDbAsync(token);
                await _authRepo.SaveChangesAsync();

                return Ok(new CommunicationModel<User>() {
                    AuthResult = new AuthResult() {
                        Token = jwtToken,
                        RefreshToken = token.Token
                    },
                    GenericModel = existingUser
                });
            }
            var result = JwtTokenUtility.Result(false, "Invalid payload");
            return BadRequest(result);
        }
        
        [HttpPost("registration")]
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
                var newUser = new User(){ EMail= user.Email , ContractNumber=user.ContractNumber};
                var isValid=PasswordUtility.ValidatePassword(user.Password, out var message);
                if(!isValid)
                {
                    _result=JwtTokenUtility.Result(false,message);
                    return BadRequest(_result);
                }

                var isCreated= await _userRepo.CreateUserAsync(newUser);
                await _userRepo.SaveChanges();
                
                var existRole=await _rolerepo.GetRoleByRoleNamed("admin");
                UserRole role= new UserRole(){UserId=newUser.Id,RoleId=existRole.Id};
                
                await _rolerepo.CreateRole(role);
                var token=JwtTokenUtility.GenerateJwtToken(newUser,existRole,_jwtConfig, _authRepo, out var jwtToken);
                await _authRepo.AddToDbAsync(token);
                await _authRepo.SaveChangesAsync();
                var pw=PasswordUtility.GenerateSaltedHash(10, user.Password);
                pw.UserId=newUser.Id;
                
                await _passwordRepo.CreatePassword(pw);
                await _passwordRepo.SaveChanges();
                return Ok(new CommunicationModel<User>(){
                    AuthResult= new AuthResult(){
                        Token=jwtToken,
                        RefreshToken=token.Token,
                        Success=true
                        },
                    GenericModel= newUser
                });
            }

            _result=JwtTokenUtility.Result(false,"Invalid payload");
            return BadRequest(_result);
        }

        [HttpPost("logOut")]
        public async Task<IActionResult> LogOut([FromBody] TokenRequest tokenRequest){
            var token=await _authRepo.GetToken(tokenRequest.RefreshToken);
            _authRepo.DeleteToken(token);
            await _authRepo.SaveChangesAsync();
            _result=JwtTokenUtility.Result(false,"User is LogOut");
            
            return Ok(_result);
        }
    }
}