using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<DataAccessLayer.Models.Url> UrlList { get; set; } = default!;
        public DbSet<DataAccessLayer.Models.User> UserList { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u => u.UserName);
            modelBuilder.Entity<Url>().HasIndex(u => new { u.UserId, u.ShortUrl });
        }
    }
}