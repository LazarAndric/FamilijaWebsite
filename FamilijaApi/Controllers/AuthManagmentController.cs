using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FailijaApi.Data;
using FamilijaApi.Configuration;
using FamilijaApi.Data;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.DTOs.Response;
using FamilijaApi.Models;
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
        private readonly IUserRepoTemp _userRepotemp;
        private readonly Jwtconfig _jwtConfig;

        public AuthManagmentController(IUserRepoTemp userRepotmp, IOptionsMonitor<Jwtconfig> optionsMonitor)
        {
            _userRepotemp=userRepotmp;
            _jwtConfig=optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user){
            if(ModelState.IsValid){
                var existingUser= await _userRepotemp.FindByEmailAsync(user.Email);

                if(existingUser==null){
                    return BadRequest(new RegistrationResponse(){
                        Errors= new List<string>(){
                            "Invalid login request"
                            },
                        Success=false
                    });
                }

                var isCorrect=_userRepotemp.CheckPassword(existingUser, user.Password);
                
                if(!isCorrect)
                {
                    return BadRequest(new RegistrationResponse(){
                        Errors= new List<string>(){
                            "Invalid login requests"
                        },
                        Success=false
                    });
                }
                
                var jwtToken= GenerateJwtToken(existingUser);

                return Ok(new RegistrationResponse(){
                    Success= true,
                    Token=jwtToken
                });
            }
            return BadRequest(new RegistrationResponse(){
                Errors= new List<string>(){
                    "Invalid payload"
                },
                Success=false
                });
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto user)
        {
            if(ModelState.IsValid)
            {
                var existing = await _userRepotemp.FindByEmailAsync(user.Email);

                if(existing!=null){
                    return BadRequest(new RegistrationResponse(){
                        Errors= new List<string>(){
                            "Email already in use"
                            },
                        Success=false
                    });
                }
                var newUser = new User(){ EMail= user.Email};
                var isCreated= _userRepotemp.CreateUser(newUser);
                    var jwtToken=GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponse(){
                        Success=true,
                        Token=jwtToken
                    });
            }
        
            return BadRequest(new RegistrationResponse(){
                Errors= new List<string>(){
                    "Invalid payload"
                },
                Success=false
                });
        }

        private string GenerateJwtToken(User user){
            var jwtTokentHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(new []
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.EMail),
                    new Claim(JwtRegisteredClaimNames.Sub, user.EMail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token= jwtTokentHandler.CreateToken(tokenDescriptor);
            var jwtToken= jwtTokentHandler.WriteToken(token);

            return jwtToken;
        }
    }
}