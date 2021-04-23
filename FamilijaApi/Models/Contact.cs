using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class Contact
    {
        [Key]
        public int UserId { get; set; }
        public int CountryCode { get; set; }
        public string AreaCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}
