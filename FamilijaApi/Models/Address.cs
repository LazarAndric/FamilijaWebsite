using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FamilijaApi.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Place { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
