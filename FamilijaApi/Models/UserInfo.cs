namespace FamilijaApi.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public int ReferralId { get; set; }
        public User ReferralUser { get; set; }
        public string ContractNumber { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}