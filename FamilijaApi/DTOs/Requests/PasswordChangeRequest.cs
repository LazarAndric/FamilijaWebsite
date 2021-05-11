using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs.Requests
{
    public class PasswordChangeRequest
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
        
    }
}