using System;
using System.ComponentModel.DataAnnotations;
using FamilijaApi.Models;

namespace FamilijaApi.DTOs
{
    public class UserDeleteDto
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        [Required]
        public string Password { get; set; }
        public Contact Contact { get; set; }
        public UserInfo UserInfo { get; set; }
        [Required]
        public PersonalInfo Info { get; set; }
        public Address Address { get; set; }
    }
}