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
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        public string Password { get; set; }
        // public Contact Contact { get; set; }
        // public UserInfo UserInfo { get; set; }
        // public PersonalInfo Info { get; set; }
        // public Address Address { get; set; }
        [ForeignKey("ReferralUser")]
        public int ReferralId { get; set; }
        public User ReferralUser { get; set; }
        public string ContractNumber { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}