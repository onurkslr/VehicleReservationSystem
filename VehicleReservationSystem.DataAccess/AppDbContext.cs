using Microsoft.EntityFrameworkCore;
using VehicleReservationSystem.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace VehicleReservationSystem.DataAccess
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             base.OnModelCreating(modelBuilder); // Bu satır en üstte olmalı
            // Vehicle tablosu ayarları
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Name).IsRequired().HasMaxLength(50);
                entity.Property(v => v.IsDeleted).IsRequired().HasMaxLength(50);
                entity.Property(v => v.PlateNumber).IsRequired().HasMaxLength(20);
            });

            // Reservation tablosu ayarları
            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.UserName).IsRequired().HasMaxLength(100);
                entity.Property(r => r.VehicleId);
                entity.Property(r => r.StartDate).IsRequired().HasColumnType("timestamp without time zone");
                entity.Property(r => r.EndDate).IsRequired().HasColumnType("timestamp without time zone");

                // İlişki tanımı
                entity.HasOne(r => r.Vehicle)
                      .WithMany(v => v.Reservations)
                      .HasForeignKey(r => r.VehicleId);
            });
        }
    }
}