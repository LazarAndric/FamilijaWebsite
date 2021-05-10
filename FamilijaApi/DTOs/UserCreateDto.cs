using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{
    public class UserCreateDto
    {
        //[Required]
        //[EmailAddress]
        public string EMail { get; set; }
        public string Username { get; set; }

        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
    }
}