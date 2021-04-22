using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class UserCreateDto
    {
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        [Required]
        public string Password { get; set; }
    }
}