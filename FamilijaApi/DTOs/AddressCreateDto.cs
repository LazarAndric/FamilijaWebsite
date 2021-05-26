using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class AddressCreateDto
    {
        [Required]
        public int UserId { get; set; }
        public string Place { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Country { get; set; }
        public int PostalCode { get; set; }
    }
}
