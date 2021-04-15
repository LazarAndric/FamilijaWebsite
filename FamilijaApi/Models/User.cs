using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
        [EmailAddress]
        public string EMail { get; set; }
        [Phone]
        public string Phone { get; set; }
    }
}