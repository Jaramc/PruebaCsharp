using Microsoft.EntityFrameworkCore;
using PruebaCsharp.Models;

namespace PruebaCsharp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SportsFacility> SportsFacilities { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .HasMaxLength(60);

            modelBuilder.Entity<User>()
                .Property(u => u.DocumentId)
                .HasMaxLength(30);

            modelBuilder.Entity<User>()
                .Property(u => u.Phone)
                .HasMaxLength(15);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .HasMaxLength(80);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.DocumentId)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<SportsFacility>()
                .Property(s => s.Name)
                .HasMaxLength(30);

            modelBuilder.Entity<SportsFacility>()
                .HasIndex(s => new { s.Name, s.TypeOfSpace })
                .IsUnique();

            modelBuilder.Entity<SportsFacility>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_SportsFacility_Capacity",
                    "Capacity > 0"
                ));

            modelBuilder.Entity<Reservation>()
                .ToTable(t => t.HasCheckConstraint(
                    "CK_Reservation_Time",
                    "EndTime > StartTime"
                ));

            modelBuilder.Entity<Notification>()
                .Property(n => n.EmailRecipient)
                .HasMaxLength(80);

            modelBuilder.Entity<Notification>()
                .Property(n => n.Subject)
                .HasMaxLength(50);

            modelBuilder.Entity<Notification>()
                .Property(n => n.ErrorMessage)
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Reservations)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SportsFacility>()
                .HasMany(s => s.Reservations)
                .WithOne(r => r.SportsFacility)
                .HasForeignKey(r => r.SportsFacilityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reservation>()
                .HasMany(r => r.Notifications)
                .WithOne(n => n.Reservation)
                .HasForeignKey(n => n.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}