using Microsoft.EntityFrameworkCore;
using FamilijaApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FamilijaApi.Data
{
    public class FamilijaDbContext : IdentityDbContext
    {
        public DbSet<User> User {get; set;}
        public DbSet<Role> Role {get; set;}
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Address> Address { get; set; }

        public FamilijaDbContext(DbContextOptions<FamilijaDbContext> options) : base (options)
        {
        }

        
    }
}