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
        public string Username { get; set; }
        [EmailAddress]
        public string EMail { get; set; }
        //public string Password { get; set; }
        // public Contact Contact { get; set; }
        // public UserInfo UserInfo { get; set; }
        // public PersonalInfo Info { get; set; }
        // public Address Address { get; set; }
        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
        //public bool IsConfirmed { get; set; }
    }
}