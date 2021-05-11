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

namespace FamilijaApi.Utility
{
    public static class JwtTokenUtility
    {
        public static RefreshToken GenerateJwtToken(
            User user, 
            Role role, 
            Jwtconfig jwtConfig, 
            IAuthRepo authRepo,
            out string jwtToken
            )
        {
            var jwtTokentHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(new []
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Value),
                    new Claim(JwtRegisteredClaimNames.Email, user.EMail),
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

        
        public static async Task<AuthResult> VerifyAndGenerateToken(
            TokenRequest tokenRequest, 
            TokenValidationParameters tokenValidation, 
            IAuthRepo authRepo, 
            IUserRepo userRepo,
            IRoleRepo roleRepo,
            Jwtconfig jwtConfig
            )
        {
            var jwtTokentHandler= new JwtSecurityTokenHandler();

            try
            {
                //Validation 1 - validate jwt token format
                var tokenInVerification = jwtTokentHandler.ValidateToken(tokenRequest.Token, tokenValidation, out var validatedToken);
                
                //Validation 2 - validate encryption alg
                if(validatedToken is JwtSecurityToken jwtSecurityToken){
                    var result= jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                    if(!result)
                        return null;
                }

                //Validation 3 - Stored
                var storedToken = await authRepo.GetToken(tokenRequest.RefreshToken);
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
                var auth=new AuthResult();
                if(expiryDate> curDate)
                {
                    auth= new AuthResult(){
                        Token=tokenRequest.Token,
                        RefreshToken=tokenRequest.RefreshToken,
                        Success=true
                    };                    
                }
                else
                {
                    var user = await userRepo.GetUserByIdAsync(int.Parse(tokenInVerification.Identity.Name));
                    if(user==null)
                        return Result(false, "Token name identifier it's not work");
                    var userRole= await roleRepo.GetRole(user.Id);
                    if(userRole==null)
                        return Result(false, "Role is not founded");
                    var existRole= await roleRepo.GetRoleByRoleNamed(role);
                    var token= GenerateJwtToken(user, existRole, jwtConfig, authRepo, out var jwtToken);
                    
                    await authRepo.UpdateTokenAsync(user.Id, token);
                    await authRepo.SaveChangesAsync();
                    auth= new AuthResult(){
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

        private static string RandomString(int length)
        {
            var random=new Random();
            var chars= "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x=>x[random.Next(x.Length)]).ToArray());
        }

        private static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            var dateTime = new DateTime(1970, 1,1,0,0,0,0, DateTimeKind.Utc);
            dateTime= dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        
        public static AuthResult Result(bool isSucess, string message){
            return new AuthResult(){
                    Success=isSucess,
                    Errors=new List<string>(){
                        message
                    }
                };
        }
    }
}