using FailijaApi.Data;
using FamilijaApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace FamilijaApi.Data
{
    public class SqlContactRepo : IContactRepo
    {
        private FamilijaDbContext _context;
        public SqlContactRepo(FamilijaDbContext context){
            _context= context;
        }

        public async void CreateContact(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
        }

        public void DeleteContact(Contact deleteModelContact)
        {
            if (deleteModelContact == null)
            {
                throw new ArgumentNullException(nameof(deleteModelContact));
            }
            _context.Contacts.Remove(deleteModelContact);
        }

        public async Task<Contact> GetContact(int id)
        {
            return await _context.Contacts.FirstOrDefaultAsync(item => item.UserId == id);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public void UpdateContact(Contact updateModelContact)
        {

        }
    }
}