using FamilijaApi.Models;

namespace FamilijaApi.DTOs
{
    public class UserInfoCreateDto
    {
        public int ReferralId { get; set; }
        public string ContractNumber { get; set; }
        public int RoleId { get; set; }
    }
}