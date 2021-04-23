using FamilijaApi.Models;

namespace FamilijaApi.DTOs
{
    public class UserInfoReadDto
    {
        public int UserId { get; set; }
        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
        public Role Role { get; set; }
    }
}