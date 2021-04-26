using System.Collections.Generic;
using FamilijaApi.Models;
using System.Threading.Tasks;

namespace FailijaApi.Data
{
    public interface IContactRepo
    {
        Task<Contact> GetContact(int id);
        void CreateContact(Contact contact);
        Task<bool> SaveChanges();
        void UpdateContact(Contact updateModelContact);
        void DeleteContact(Contact deleteModelContact);
    }
}