using System;

namespace FamilijaApi.DTOs
{
    public class RefreshTokenValidate
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string RefreshToken { get; set; }
        public string JwtId { get; set; }
    }
}