using Microsoft.EntityFrameworkCore;
using FamilijaApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FamilijaApi.Data
{
    public class FamilijaDbContext : DbContext
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles {get; set;}
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Finance> Finance { get; set; }

        public FamilijaDbContext(DbContextOptions<FamilijaDbContext> options) : base (options)
        {
        }

        
    }
}