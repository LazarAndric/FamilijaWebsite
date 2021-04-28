using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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
        private readonly IMapper _mapper;
        private readonly IAuthRepo _authRepo;
        private readonly IUserRepoTemp _userRepotemp;
        private readonly Jwtconfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidation;

        public AuthManagmentController(TokenValidationParameters tokenValidation, IMapper mapper, IAuthRepo authRepo, IUserRepoTemp userRepotmp, IOptionsMonitor<Jwtconfig> optionsMonitor)
        {
            _tokenValidation=tokenValidation;
            _mapper = mapper;
            _authRepo =authRepo;
            _userRepotemp=userRepotmp;
            _jwtConfig=optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
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
                
                var jwtToken= await GenerateJwtToken(existingUser);

                return Ok(jwtToken);
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
        [AllowAnonymous]
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
                    var jwtToken=await GenerateJwtToken(newUser);
                    return Ok(jwtToken);
            }
        
            return BadRequest(new RegistrationResponse(){
                Errors= new List<string>(){
                    "Invalid payload"
                },
                Success=false
                });
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest){
            if(ModelState.IsValid)
            {
                var result = await VerifAndGenerateToken(tokenRequest);

                if(result == null)
                {
                    return BadRequest(new RegistrationResponse()
                    {
                        Errors = new List<string>(){
                            "Invalid payload"
                        },
                        Success = false
                    });
                }

                return Ok(result);
            }
            return BadRequest(new RegistrationResponse(){
               Errors=  new List<string>(){
                   "Invalid payload"
               },
               Success=false
            });
        }

        private async Task<AuthResult> VerifAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokentHandler= new JwtSecurityTokenHandler();

            try
            {
                // var exp=jwtTokentHandler.ReadJwtToken(tokenRequest.Token).ValidTo.ToLocalTime();
                // var curDate=DateTime.UtcNow.ToLocalTime();
                // if(exp<curDate){
                //     return new AuthResult(){
                //         Success=false,
                //         Errors= new List<string>(){
                //             "Token has expired"
                //         }
                //     };
                // }
                //Validation 1 - validate jwt token format
                var tokenInVerification = jwtTokentHandler.ValidateToken(tokenRequest.Token, _tokenValidation, out var validatedToken);

                //Validation 2 - validate encryption alg
                if(validatedToken is JwtSecurityToken jwtSecurityToken){
                    var result= jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if(!result){
                        return null;
                    }
                }
                //Validation 3 - Expiry
                var utcExpiryDate= long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                var curDate= DateTime.UtcNow.ToLocalTime();
                if(expiryDate> curDate){
                    return new AuthResult(){
                        Success=false,
                        Errors= new List<string>(){
                            "Token has not yet expired"
                        }
                    };
                }

                //Validation 4 - Stored
                var storedToken = await _authRepo.GetToken(tokenRequest.RefreshToken);

                if (storedToken == null)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Token does not exist"
                        }
                    };
                }

                //Validation 5 - Used
                if (storedToken.IsUsed)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Token has been used"
                        }
                    };
                }

                //Validation 6 - Revoked
                if (storedToken.IsRevoked)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Token has been revoked"
                        }
                    };
                }


                //Validation 7
                var jti = tokenInVerification.Claims.FirstOrDefault(x=>x.Type== JwtRegisteredClaimNames.Jti).Value;

                if(storedToken.JwtId != jti)
                {
                    return new AuthResult()
                    {
                        Success = false,
                        Errors = new List<string>(){
                            "Token doesn't match"
                        }
                    };
                }

                //update current token
                storedToken.IsUsed = true;
                _authRepo.DeleteToken(storedToken);
                await _authRepo.SaveChangesAsync();

                var dbUser = await _userRepotemp.FindByIdAsync(storedToken.UserId);
                return await GenerateJwtToken(dbUser);

            }
            catch (Exception ex)
            {
                return new AuthResult(){
                    Success=false,
                    Errors=new List<string>(){
                        ex.Message
                    }
                };
            }
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1,1,0,0,0,0, DateTimeKind.Utc);
            dateTime= dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }

        private async Task<AuthResult> GenerateJwtToken(User user){
            var jwtTokentHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(new []
                {
                    new Claim("Id", user.EMail),
                    new Claim(JwtRegisteredClaimNames.Email, user.EMail),
                    new Claim(JwtRegisteredClaimNames.Sub, user.EMail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token= jwtTokentHandler.CreateToken(tokenDescriptor);
            var jwtToken= jwtTokentHandler.WriteToken(token);

            var refreshToken= new RefreshToken(){
                JwtId= token.Id,
                IsUsed=false,
                IsRevoked=false,
                UserId= user.Id,
                AddedDate= DateTime.UtcNow,
                ExpiryDate=DateTime.UtcNow.AddMonths(6),
                Token=RandomString(35)+Guid.NewGuid()
            };
            await _authRepo.AddToDbAsync(refreshToken);
            await _authRepo.SaveChangesAsync();

            return new AuthResult(){
                Token= jwtToken,
                Success=true,
                RefreshToken=refreshToken.Token
            };
        }

        private string RandomString(int length)
        {
            var random=new Random();
            var chars= "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x=>x[random.Next(x.Length)]).ToArray());
        }
    }
}