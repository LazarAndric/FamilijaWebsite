using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class AddressUpdateDto
    {
        [Required]
        public string Place { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public int PostalCode { get; set; }
    }
}
