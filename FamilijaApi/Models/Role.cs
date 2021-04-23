using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string Value { get; set; }
    }
}