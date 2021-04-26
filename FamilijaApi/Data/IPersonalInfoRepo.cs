using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IPersonalInfoRepo
    {
        Task<PersonalInfo> GetPersonalInfo(int id);
        void CreatePersonalInfo(PersonalInfo personalInfo);
        Task<bool> SaveChanges();
    }
}