using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class UserCreateDto
    {
        [Required]
        public string EMail { get; set; }
        public string PhoneNumber { get; set; }
        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
    }
}