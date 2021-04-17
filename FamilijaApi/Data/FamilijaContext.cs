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
    }
}