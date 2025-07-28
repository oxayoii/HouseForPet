using DataBaseContext.Models;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DataBaseContext
{
    public class DataBasePrimaryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DataBasePrimaryContext(DbContextOptions<DataBasePrimaryContext> options) : base (options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
