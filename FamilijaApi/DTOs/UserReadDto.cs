using System;
using System.ComponentModel.DataAnnotations;
using FamilijaApi.Models;

namespace FamilijaApi.DTOs
{
    public class UserReadDto
    {
        public int Id { get; set; }
        [EmailAddress]
        public string EMail { get; set; }
        public bool EmailConfirmed { get; set; }
        public int ReferralId { get; set; }
        public User ReferralUser { get; set; }
        public string ContractNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string ReferralCode { get; set; }


        //models
        public PersonalInfoReadDto Info { get; set; }
        public AddressReadDto Address { get; set; }
    }
}