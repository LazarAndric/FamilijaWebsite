using Microsoft.EntityFrameworkCore;
using FamilijaApi.Models;

namespace FamilijaApi.Data
{
    public class FamilijaContext : DbContext
    {
        public FamilijaContext(DbContextOptions<FamilijaContext> options) : base (options)
        {
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles {get; set;}
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
    }
}