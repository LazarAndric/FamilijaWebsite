using FamilijaApi.Models;

namespace FamilijaApi.DTOs
{
    public class JwtTokenValidate
    {
        public string JwtId { get; set; }
        public string Error { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }
        public bool Success { get; set; }
        public bool IsExpiry { get; set; }
    }
}