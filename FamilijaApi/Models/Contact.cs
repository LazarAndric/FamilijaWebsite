using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilijaApi.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
