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
        Username = "LECT1",
        PasswordHash = "pass",
        FirstName = "Lerato",
        LastName = "Dlamini",
        Email = "lecturer@example.com",
        HourlyRate = 200,
        Role = UserRole.Lecturer
    },
    new AppUser
    {
        Id = 2,
        Username = "COORD1",
        PasswordHash = "pass",
        FirstName = "Sifiso",
        LastName = "Nkosi",
        Email = "coord@example.com",
        HourlyRate = 0,
        Role = UserRole.Coordinator
    },
    new AppUser
    {
        Id = 3,
        Username = "MANAGER1",
        PasswordHash = "pass",
        FirstName = "Thuli",
        LastName = "Zulu",
        Email = "manager@example.com",
        HourlyRate = 0,
        Role = UserRole.Manager
    },
    new AppUser
    {
        Id = 4,
        Username = "HR1",
        PasswordHash = "pass",
        FirstName = "John",
        LastName = "Moyo",
        Email = "hr@example.com",
        HourlyRate = 0,
        Role = UserRole.HR
    }


            );
        }

    }
}
