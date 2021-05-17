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
        private IMapper _mapper;
        private readonly IRoleRepo _roleRepo;
        private readonly IAuthRepo _authRepo;
        private readonly IUserRepo _userRepo;
        private readonly IPasswordRepo _passwordRepo;
        private readonly JwtTokenUtility _jwtTokenUtil;

        public AuthorizationsController(
            IMapper mapper,
            IPasswordRepo passwordRepo, 
            IRoleRepo roleRepo, 
            TokenValidationParameters tokenValidation,
            IAuthRepo authRepo, 
            IUserRepo userRepo, 
            IOptionsMonitor<Jwtconfig> optionsMonitor
            )
        {
            _mapper=mapper;
            _passwordRepo = passwordRepo;
            _roleRepo = roleRepo;
            _authRepo = authRepo;
            _userRepo = userRepo;
            _jwtTokenUtil=new JwtTokenUtility(authRepo, userRepo, roleRepo, optionsMonitor.CurrentValue, tokenValidation);
        }

        [HttpPost("logIn")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user) {
            try
            {
                if (ModelState.IsValid) {
                    var existingUser = new User();
                    if (MailAddress.TryCreate(user.Username, out var mail))
                        existingUser = await _userRepo.FindByEmailAsync(mail.Address);
                    if (existingUser == null)
                    {
                        throw new Exception("Your e-mail or password is wrong");
                    }
                    var pass = await _passwordRepo.GetPasswordAsync(existingUser.Id);
                    var isCorrect = PasswordUtility.VerifyPassword(user.Password, pass.Hash, pass.Salt);

                    if (!isCorrect)
                    {
                        throw new Exception("Your e-mail or password is wrong");
                    }

                    var existRole = await _roleRepo.GetRole(existingUser.Id);
                    var role = await _roleRepo.GetRoleByRoleId(existRole.RoleId);
                    var token = _jwtTokenUtil.GenerateJwtToken(existingUser, role, out var jwtToken);

                    await _authRepo.AddToDbAsync(token);
                    await _authRepo.SaveChangesAsync();

                    return Ok(new CommunicationModel {
                        Result=new AuthResult(){
                            Success=true
                        },
                        CreateToken= new AuthTokenCreate(){
                            RefreshToken=token.Token,
                            JwtToken=jwtToken
                        }
                    });
                }
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new AuthResult(){
                    Errors=new List<string>(){
                        ex.Message
                    },
                    Success=false
                });
            }
        }
        
        [HttpPost("registration")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var existing = await _userRepo.FindByEmailAsync(user.Email);
                    if(existing!=null){
                        throw new Exception("E-mail already in use");
                    }

                    var refId = await _userRepo.FindReferalbyCodeAsync(user.SponsorCode);
                    if(refId==null)
                        refId= await _userRepo.GetUserByIdAsync(74);
                    var newUser = new User() { EMail = user.Email, ContractNumber = user.ContractNumber, ReferralCode = JwtTokenUtility.RandomString(6), ReferralId = refId.Id, DateRegistration=DateTime.UtcNow.ToLocalTime() };
                    var isValid=PasswordUtility.ValidatePassword(user.Password, out var message);
                    if(!isValid)
                    {
                        throw new Exception(message);
                    }

                    var isCreated= await _userRepo.CreateUserAsync(newUser);
                    await _userRepo.SaveChangesAsync();
                    
                    var existRole=await _roleRepo.GetRoleByRoleNamed("admin");
                    UserRole role= new UserRole(){UserId=newUser.Id,RoleId=existRole.Id};
                    
                    await _roleRepo.CreateRole(role);
                    var token = _jwtTokenUtil.GenerateJwtToken(newUser, existRole, out var jwtToken);
                    await _authRepo.AddToDbAsync(token);
                    await _authRepo.SaveChangesAsync();
                    var pw=PasswordUtility.GenerateSaltedHash(10, user.Password);
                    pw.UserId=newUser.Id;
                    await _passwordRepo.CreatePasswordAsync(pw);
                    await _passwordRepo.SaveChangesAsync();
                    return Ok(new CommunicationModel(){
                        CreateToken= new AuthTokenCreate(){
                            JwtToken=jwtToken,
                            RefreshToken=token.Token
                            },
                        Result= new AuthResult(){
                            Success=true
                        }
                    });
                }
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return NotFound(new AuthResult(){
                    Success=false,
                    Errors=new List<string>(){
                        ex.Message
                        }
                });
            }
            
        }

        [HttpPost("VerifyToken")]
        public async Task<IActionResult> VerifyToken([FromHeader] string authorization, [FromHeader] string authorizationRefresh){
            var str=authorizationRefresh;
            try
            {
                var auth = await _jwtTokenUtil.VerifyRefreshToken(authorizationRefresh);
                if(!auth.Success)
                    throw new Exception(auth.Error);
                
                var authJwt= await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(!authJwt.Success)
                    throw new Exception(auth.Error);
                if(!JwtTokenUtility.IsIdValid(auth.JwtId, authJwt.JwtId))
                    throw new Exception("JTI Id is not valid");
                
                if(authJwt.IsExpiry){
                    var newJwt = _jwtTokenUtil.GenerateJwtToken(authJwt.User, authJwt.Role, out string newJwtToken);
                    await _authRepo.UpdateTokenAsync(authJwt.User.Id, newJwt);
                    await _authRepo.SaveChangesAsync();

                    return Created("", true);
                }

                return Ok(
                    new AuthResult(){
                        Errors= new List<string>(){
                            auth.Error
                        },
                        Success=  auth.Success,
                });
            }
            catch (System.Exception ex)
            {
                return Ok(
                    new AuthResult(){
                        Errors= new List<string>(){
                            ex.Message
                            },
                        Success=  false,
                });
            }
        }

        [HttpPost("logOut")]
        public async Task<IActionResult> LogOut([FromHeader] string authorization){
            try
            {
                var auth= await _jwtTokenUtil.VerifyJwtToken(authorization);
                if(!auth.Success){
                    throw new Exception("Token doesn't exist");
                }
                var token= await _authRepo.GetTokenByuserIdAsync(auth.User.Id);
                _authRepo.DeleteToken(token);
                await _authRepo.SaveChangesAsync();
                
                return Ok(new AuthResult(){
                    Success=true
                });
            }
            catch (System.Exception ex)
            {
                return NotFound(new AuthResult(){
                    Errors=new List<string>(){
                        ex.Message
                        },
                    Success=false
                });
            }
            
        }

    }
}