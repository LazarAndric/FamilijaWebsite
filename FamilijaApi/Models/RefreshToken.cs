using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FamilijaApi.Models
{
    public class RefreshToken
    {
        [Key]
        public int UserId { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}