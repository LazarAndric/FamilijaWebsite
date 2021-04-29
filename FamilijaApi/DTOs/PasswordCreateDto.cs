using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.DTOs
{
    public class PasswordCreateDto
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
