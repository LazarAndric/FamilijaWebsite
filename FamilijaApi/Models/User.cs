using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class User
    {
        [Key]
        public int  Id { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        // public Contact Contact { get; set; }
        // public UserInfo UserInfo { get; set; }
        // public PersonalInfo Info { get; set; }
        // public Address Address { get; set; }
        public int ReferralId { get; set; }
        //public User ReferralUser { get; set; }
        public string ContractNumber { get; set; }
        public int RoleId { get; set; }
        //public Role Role { get; set; }
    }
}