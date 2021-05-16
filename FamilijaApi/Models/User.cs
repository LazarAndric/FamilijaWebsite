using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FamilijaApi.Models
{
    public class User
    {
        [Key]
        public int  Id { get; set; }
        [EmailAddress]
        public string EMail { get; set; }
        public bool EmailConfirmed { get; set; }
        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ReferralCode { get; set; }
        public DateTime DateRegistration { get; set; }

    }
}