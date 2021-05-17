using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IPersonalInfoRepo
    {
        Task<PersonalInfo> GetPersonalInfoAsync(int userId);
        void CreatePersonalInfo(PersonalInfo personalInfo);
        Task<bool> SaveChanges();
        void UpdatePersonalInfo(PersonalInfo updateModelPersonalInfo);
        void DeletePersonalInfo(PersonalInfo deleteModelPersonalInfo);
    }
}