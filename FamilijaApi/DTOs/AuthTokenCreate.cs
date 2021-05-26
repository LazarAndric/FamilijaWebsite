using FamilijaApi.Configuration;

namespace FamilijaApi.DTOs
{
    public class AuthTokenCreate
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}