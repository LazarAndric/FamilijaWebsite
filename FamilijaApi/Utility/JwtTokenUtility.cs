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

namespace FamilijaApi.Utility
{
    public static class JwtTokenUtility
    {
        public static async Task<AuthResult> GenerateJwtToken(
            User user, 
            Role role, 
            Jwtconfig jwtConfig, 
            IAuthRepo authRepo
            )
        {
            var jwtTokentHandler = new JwtSecurityTokenHandler();

            var strKey = Base64UrlEncoder.Encode(jwtConfig.Secret);
            var key = Encoding.Default.GetBytes(strKey);
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject= new ClaimsIdentity(new []
                {
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                    new Claim(ClaimTypes.Role, role.Value),
                    new Claim(JwtRegisteredClaimNames.Email, user.EMail),
                    new Claim(JwtRegisteredClaimNames.Sub, user.EMail),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
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
            await authRepo.AddToDbAsync(refreshToken);
            await authRepo.SaveChangesAsync();

            return new AuthResult(){
                Token= jwtToken,
                Success=true,
                RefreshToken=refreshToken.Token
            };
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
                    var result= jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

                    if(!result)     return null;
                }
                //Validation 3 - Expiry
                var utcExpiryDate= long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Exp).Value);

                var expiryDate = UnixTimeStampToDateTime(utcExpiryDate);
                var curDate= DateTime.UtcNow.ToLocalTime();
                if(expiryDate> curDate)    return Result(false, "Token has not yet expired");

                //Validation 4 - Stored
                var storedToken = await authRepo.GetToken(tokenRequest.RefreshToken);
                if (storedToken == null)    return Result(false, "Token does not exist");

                //Validation 5 - Used
                if (storedToken.IsUsed)     return Result(false, "Token has been used");

                //Validation 6 - Revoked
                if(storedToken.IsRevoked)   return Result(false, "Token has been revoked");


                //Validation 7
                var jti = tokenInVerification.Claims.FirstOrDefault(x=>x.Type== JwtRegisteredClaimNames.Jti).Value;

                if(storedToken.JwtId != jti)
                {
                    return Result(false, "Token doesn't match");
                }

                //update current token
                storedToken.IsUsed = true;
                authRepo.DeleteToken(storedToken);
                await authRepo.SaveChangesAsync();

                var dbUser = await userRepo.GetUserByIdAsync(storedToken.UserId);
                var existRole= await roleRepo.GetRole(dbUser.Id);
                var role= await roleRepo.GetRoleByRoleId(existRole.RoleId);
                return await GenerateJwtToken(dbUser, role, jwtConfig, authRepo);
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