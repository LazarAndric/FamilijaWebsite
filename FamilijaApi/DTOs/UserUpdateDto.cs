using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class UserUpdateDto
    {
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        [Required]
        public string ContractNumber { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}