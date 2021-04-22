using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class ContactReadDto
    {
        public int UserId { get; set; }
        [Required]
        public int CountryCode { get; set; }
        [Required]
        public string AreaCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}