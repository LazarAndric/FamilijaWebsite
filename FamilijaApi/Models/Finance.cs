using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class Finance
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public double TotalSpent { get; set; }
    }
}
