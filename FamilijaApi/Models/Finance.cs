using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.Models
{
    public class Finance
    {
        [Key]
        public int UserId { get; set; }
        public float TotalSpent { get; set; }
    }
}
