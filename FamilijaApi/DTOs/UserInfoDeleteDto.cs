using FamilijaApi.Models;

namespace FamilijaApi.DTOs
{
    public class UserInfoDeleteDto
    {
        public int UserId { get; set; }
        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
        public int RoleId { get; set; }
    }
}