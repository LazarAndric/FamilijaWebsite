using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs.Requests
{
    public class UserLoginRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}