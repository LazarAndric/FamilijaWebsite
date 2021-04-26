using System.ComponentModel.DataAnnotations;


namespace FamilijaApi.DTOs
{
    public class FinanceReadDto
    {
        public int UserId { get; set; }
        public double TotalSpent { get; set; }
    }
}
