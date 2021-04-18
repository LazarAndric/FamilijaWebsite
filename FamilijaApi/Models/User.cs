using System;
using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public int Age { get; set; }
    }
}