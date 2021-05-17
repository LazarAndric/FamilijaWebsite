using System.Threading.Tasks;
using FamilijaApi.Configuration;
using FamilijaApi.DTOs;
using FamilijaApi.DTOs.Requests;
using FamilijaApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace FamilijaApi.Data
{
    public interface IJwtUtil
    {
        public RefreshToken GenerateJwtToken(User user, Role role, out string jwtToken);
        public Task<RefreshTokenValidate> VerifyRefreshToken(string refreshToken);
        Task<JwtTokenValidate> VerifyJwtToken(string jwtToken);
    }
}