using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs.Requests
{
    public class UserRegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ContractNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string SponsorCode { get; set; }





    }
}