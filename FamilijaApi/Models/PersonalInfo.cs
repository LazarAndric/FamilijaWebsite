using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class PersonalInfo
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
