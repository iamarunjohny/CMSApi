using CMSApi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMSApi.DAL
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Contact>().HasKey(c => c.ContactId);
            modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.Id);

            // Add more configurations here if necessary
        }
    }
}
