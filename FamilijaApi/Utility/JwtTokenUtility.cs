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

        public static bool IsIdValid(string storedJti, string jti){
            return storedJti.Equals(jti);
        }

        public async Task<JwtTokenValidate> VerifyJwtToken(string jwtToken){
            try
            {
                jwtToken= jwtToken.Remove(0, 7);
                var jwtTokentHandler= new JwtSecurityTokenHandler();

                var tokenInVerification = jwtTokentHandler.ValidateToken(jwtToken, _tokenValidation, out var validatedToken);
                
                if(validatedToken is JwtSecurityToken jwtSecurityToken){
                    var result= jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                    if(!result)
                        throw new Exception("Algorithm not validate");

                    var utcExpiryDate= long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>x.Type==JwtRegisteredClaimNames.Exp).Value);

                    var id = int.Parse(tokenInVerification.Identity.Name);
                    var user= await _userRepo.GetUserByIdAsync(id);
                    if(user==null)
                        throw new Exception("User not found");

                    var role = tokenInVerification.Claims.FirstOrDefault(x=>x.Type== ClaimsIdentity.DefaultRoleClaimType).Value;
                    var userRole= await _roleRepo.GetRole(user.Id);
                    var usrRole = await _roleRepo.GetRoleByRoleId(userRole.RoleId);

                    if(usrRole.Value!=role){
                        throw new Exception("User is not same");
                    }

                    var jti= tokenInVerification.Claims.FirstOrDefault(x=>x.Type== JwtRegisteredClaimNames.Jti).Value;

                    var dateTime= long.Parse(tokenInVerification.Claims.FirstOrDefault(x=>x.Type== JwtRegisteredClaimNames.Exp).Value);
                    if(UnixTimeStampToDateTime(dateTime) < DateTime.UtcNow.ToLocalTime())
                        return new JwtTokenValidate(){
                            User=user,
                            Role=usrRole,
                            Success=true,
                            IsExpiry=true,
                            JwtId=jti
                        };
                        
                    return new JwtTokenValidate(){
                        User=user,
                        Role=usrRole,
                        IsExpiry=false,
                        Success=true,
                        JwtId=jti
                    };
                }
                throw new Exception("Token is not validate");
            }
            catch (System.Exception ex)
            {
                return new JwtTokenValidate(){
                    IsExpiry=false,
                    Success=false,
                    Error=ex.Message
                };
            }
            
        }
        
        public async Task<RefreshTokenValidate> VerifyRefreshToken(string refreshToken)
        {
            try
            {
                var storedToken = await _authRepo.GetTokenAsync(refreshToken);
                if (storedToken == null)
                {
                    throw new Exception("Token does not exist");
                }

                if(storedToken.ExpiryDate < DateTime.UtcNow.ToLocalTime())
                {
                    throw new Exception("Token is expiry");
                }

                return new RefreshTokenValidate(){
                    Success=true,
                    JwtId=storedToken.JwtId
                };
            }
            catch (Exception ex)
            {
                return new RefreshTokenValidate(){
                       Success=false,
                       Error=ex.Message
                };
            }
        }

        public static string RandomString(int length)
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
    }
}