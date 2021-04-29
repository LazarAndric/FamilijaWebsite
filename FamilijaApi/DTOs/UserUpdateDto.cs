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
        //public string Password { get; set; }

        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
        public int RoleId { get; set; }
    }
}