using CMCS.Models;
using Microsoft.EntityFrameworkCore;

namespace CMCS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Claim> Claims => Set<Claim>();
        public DbSet<SupportingDocument> SupportingDocuments => Set<SupportingDocument>();
        public DbSet<AppUser> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AppUser>().HasData(

                new AppUser
                {
                    Id = 1,
                    Username = "hazelshaka",
                    PasswordHash = "pass",
                    FirstName = "hazel",
                    LastName = "shaka",
                    Email = "hazelshaka@gmail.com",
                    HourlyRate = 200,
                    Role = UserRole.Lecturer
                },

                new AppUser
                {
                    Id = 2,
                    Username = "Kim009",
                    PasswordHash = "pass",
                    FirstName = "kim",
                    LastName = "Yam",
                    Email = "jackharlow@gmail.com",
                    HourlyRate = 70,
                    Role = UserRole.Lecturer
                },

                new AppUser
                {
                    Id = 3,
                    Username = "L2001",
                    PasswordHash = "pass",
                    FirstName = "Namjoon",
                    LastName = "Rm",
                    Email = "namjoonrm@gmail.com",
                    HourlyRate = 500,
                    Role = UserRole.Lecturer
                },

                new AppUser
                {
                    Id = 4,
                    Username = "COORD1",
                    PasswordHash = "pass",
                    FirstName = "john",
                    LastName = "Baptist",
                    Email = "nickiminaj@gmail.com",
                    HourlyRate = 0,
                    Role = UserRole.Coordinator
                },

                new AppUser
                {
                    Id = 5,
                    Username = "MANAGER1",
                    PasswordHash = "pass",
                    FirstName = "Boss",
                    LastName = "Baby",
                    Email = "euphoriajung@gmail.com",
                    HourlyRate = 0,
                    Role = UserRole.Manager
                },

                new AppUser
                {
                    Id = 6,
                    Username = "HR1",
                    PasswordHash = "pass",
                    FirstName = "Queen",
                    LastName = "Bey",
                    Email = "bambamgot7@gmail.com",
                    HourlyRate = 0,
                    Role = UserRole.HR
                }
            );
        }
    }
}
