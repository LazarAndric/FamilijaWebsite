using System;
using System.ComponentModel.DataAnnotations;
using FamilijaApi.Models;

namespace FamilijaApi.DTOs
{
    public class UserReadDto
    {
        [Key]
        public int Id { get; set; }
        public string EMail { get; set; }
        //public string Password { get; set; }
        public int ReferralId { get; set; }

        public User ReferralUser { get; set; }
        public string ContractNumber { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public Contact Contact { get; set; }
        [Required]
        public PersonalInfo Info { get; set; }
        public AddressReadDto Address { get; set; }
    }
}