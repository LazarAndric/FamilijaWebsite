using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class Address
    {
        [Key]
        public int UserId { get; set; }
        public string Place { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public int PostalCode { get; set; }
    }
}
