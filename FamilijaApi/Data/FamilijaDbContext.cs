using Microsoft.EntityFrameworkCore;
using FamilijaApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace FamilijaApi.Data
{
    public class FamilijaDbContext : DbContext
    {
        public DbSet<User> Users {get; set;}
        public DbSet<Role> Roles {get; set;}
        public DbSet<PersonalInfo> PersonalInfo { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Finance> Finance { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public FamilijaDbContext(DbContextOptions<FamilijaDbContext> options) : base (options)
        {
        }
    }
}