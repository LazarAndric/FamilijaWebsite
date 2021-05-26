using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class PersonalInfoCreateDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
    }
}
