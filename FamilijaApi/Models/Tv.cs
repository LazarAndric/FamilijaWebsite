using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilijaApi.Models
{
    public class Tv
    {
        [Key]
        public int Id { get; set; }
        public string Color { get; set; }
    }
}
