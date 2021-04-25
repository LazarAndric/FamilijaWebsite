using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FamilijaApi.Configuration;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.DTOs.Response;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Jwtconfig _jwtConfig;

        public AuthManagmentController(UserManager<IdentityUser> userManager, IOptionsMonitor<Jwtconfig> optionsMonitor)
        {
            _userManager=userManager;
            _jwtConfig=optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest user){
            if(ModelState.IsValid){
                var existingUser= await _userManager.FindByEmailAsync(user.Email);

                if(existingUser==null){
                    return BadRequest(new RegistrationResponse(){
                        Errors= new List<string>(){
                            "Invalid login request"
                            },
                        Success=false
                    });
                }

                var isCorrect=await _userManager.CheckPasswordAsync(existingUser, user.Password);
                
                if(!isCorrect)
                {
                    return BadRequest(new RegistrationResponse(){
                        Errors= new List<string>(){
                            "Invalid login request"
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
                var existing = await _userManager.FindByEmailAsync(user.Email);

                if(existing!=null){
                    return BadRequest(new RegistrationResponse(){
                        Errors= new List<string>(){
                            "Email already in use"
                            },
                        Success=false
                    });
                }
                var newUser = new IdentityUser(){ Email= user.Email, UserName= user.Email};
                var isCreated = await _userManager.CreateAsync(newUser, user.Password);
                if(isCreated.Succeeded)
                {
                    var jwtToken=GenerateJwtToken(newUser);
                    return Ok(new RegistrationResponse(){
                        Success=true,
                        Token=jwtToken
                    });
                }
                return BadRequest(new RegistrationResponse(){
                        Errors= isCreated.Errors.Select(x=> x.Description).ToList(),
                        Success=false
                    });
            }
        
            return BadRequest(new RegistrationResponse(){
                Errors= new List<string>(){
                    "Invalid payload"
                },
                Success=false
                });
        }

        private string GenerateJwtToken(IdentityUser user){
            var jwtTokentHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(new []
                {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
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