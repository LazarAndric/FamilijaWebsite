using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IAdddressesRepo
    {
         Task<Address> GetAddressAsync(int userId);
        void CreateAddress(Address addres);
        Task<bool> SaveChanges();
        void UpdateAddress(Address updateModelAddress);
        void DeleteAdress(Address deleteModelAddress);
    }
}