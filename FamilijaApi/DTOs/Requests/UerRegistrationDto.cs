using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        public string Username {get; set;}
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        //[StringLength(15, ErrorMessage = "Minimum legth it's 7", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ContractNumber { get; set; }
    }
}