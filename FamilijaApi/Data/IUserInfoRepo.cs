using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IUserInfoRepo
    {
        Task<UserInfo> GetUserInfo(int id);
    }
}