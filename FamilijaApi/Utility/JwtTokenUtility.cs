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
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Models;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Microsoft.Extensions.Options;
using FamilijaApi.DTOs;

namespace FamilijaApi.Utility
{
    public class JwtTokenUtility : IJwtUtil
    {
        private readonly IAuthRepo _authRepo;
        private readonly IUserRepo _userRepo;
        private readonly IRoleRepo _roleRepo;
        private readonly Jwtconfig _jwtConfig;
        private readonly TokenValidationParameters _tokenValidation;
        public JwtTokenUtility(
        IAuthRepo authRepo,
        IUserRepo userRepo,
        IRoleRepo roleRepo,
        Jwtconfig jwtconfig,
        TokenValidationParameters tokenValidation)
        {
            _authRepo=authRepo;
            _userRepo=userRepo;
            _roleRepo=roleRepo;
            _jwtConfig=jwtconfig;
            _tokenValidation=tokenValidation;
        }

        public RefreshToken GenerateJwtToken(
            User user, 
            Role role,
            out string jwtToken
            )
        {
            var jwtTokentHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(new []
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Value),
                    new Claim(JwtRegisteredClaimNames.Email, user.EMail),
                    //new Claim(JwtRegisteredClaimNames.Iss,"FamilijaApi.com/"+user.Id.ToString()),
                    //new Claim(JwtRegisteredClaimNames.Aud, user.EMail),
                    new Claim(JwtRegisteredClaimNames.Nbf, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token= jwtTokentHandler.CreateToken(tokenDescriptor);
            jwtToken= jwtTokentHandler.WriteToken(token);

            var refreshToken= new RefreshToken(){
                JwtId= token.Id,
                UserId= user.Id,
                AddedDate= DateTime.UtcNow.ToLocalTime(),
                ExpiryDate=DateTime.UtcNow.ToLocalTime().AddHours(1),
                Token=RandomString(35)+Guid.NewGuid()
            };
            return refreshToken;
        }

        
        public async Task<AuthCreate> VerifyAndGenerateToken(TokenRequest tokenRequest)
        {
            var jwtTokentHandler= new JwtSecurityTokenHandler();

            try
            {
                //Validation 1 - validate jwt token format
                var tokenInVerification = jwtTokentHandler.ValidateToken(tokenRequest.Token, _tokenValidation, out var validatedToken);
                
                //Validation 2 - validate encryption alg
                if(validatedToken is JwtSecurityToken jwtSecurityToken){
                    var result= jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                    if(!result)
                        return null;
                }

                //Validation 3 - Stored
                var storedToken = await _authRepo.GetToken(tokenRequest.RefreshToken);
                if (storedToken == null)
                    return Result(false, "Token does not exist");

                //Validation 4 - Token match
                var jti = tokenInVerification.Claims.FirstOrDefault(x=>x.Type== JwtRegisteredClaimNames.Jti).Value;
                if(storedToken.JwtId != jti)
                {
                    return Result(false, "Token doesn't match");
                }
                var role = tokenInVerification.Claims.FirstOrDefault(x=>x.Type== ClaimsIdentity.DefaultRoleClaimType).Value;
                //Validation 4 - Expiry
                var utcExpiryDate= long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                var curDate= DateTime.UtcNow.ToLocalTime();
                var auth=new AuthCreate();
                if(expiryDate> curDate)
                {
                    auth= new AuthCreate(){
                        Id=Int32.Parse(tokenInVerification.Identity.Name),
                        Token=tokenRequest.Token,
                        RefreshToken=tokenRequest.RefreshToken,
                        Success=true
                    };                    
                }
                else
                {
                    var user = await _userRepo.GetUserByIdAsync(int.Parse(tokenInVerification.Identity.Name));
                    if(user==null)
                        return Result(false, "Token name identifier it's not work");
                    var userRole= await _roleRepo.GetRole(user.Id);
                    if(userRole==null)
                        return Result(false, "Role is not founded");
                    var existRole= await _roleRepo.GetRoleByRoleNamed(role);
                    var token= GenerateJwtToken(user, existRole, out var jwtToken);
                    await _authRepo.UpdateTokenAsync(user.Id, token);
                    await _authRepo.SaveChangesAsync();
                    auth= new AuthCreate(){
                        Id=userRole.Id,
                        Token= jwtToken,
                        Success=true,
                        RefreshToken=storedToken.Token
                    };
                }
                return auth;
            }
            catch (Exception ex)
            {
                return Result(false, ex.Message);
            }
        }

        private string RandomString(int length)
        {
            var random=new Random();
            var chars= "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x=>x[random.Next(x.Length)]).ToArray());
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1,1,0,0,0,0, DateTimeKind.Utc);
            dateTime= dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        
        public static AuthCreate Result(bool isSucess, string message){
            return new AuthCreate(){
                    Success=isSucess,
                    Errors=new List<string>(){
                        message
                    }
                };
        }
        public static AuthResult ResultPW(bool isSucess, string message){
            return new AuthResult(){
                    Success=isSucess,
                    Errors=new List<string>(){
                        message
                    }
                };
        }
    }
}