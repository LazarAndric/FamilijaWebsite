using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [EmailAddress]
        public string EMail { get; set; }
        public string Password { get; set; }
        public Contact Contact { get; set; }
        public UserInfo UserInfo { get; set; }
        public PersonalInfo Info { get; set; }
        public Address Address { get; set; }
    }
}