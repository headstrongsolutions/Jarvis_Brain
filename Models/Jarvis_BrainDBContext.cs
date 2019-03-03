using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Jarvis_Brain.Models
{
    public partial class Jarvis_BrainDBContext : DbContext
    {
        public Jarvis_BrainDBContext()
        {
        }

        public Jarvis_BrainDBContext(DbContextOptions<Jarvis_BrainDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DhtPackages> DhtPackages { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Removed connectionstring as if this throws an error it's 
                //  because the DI hasn't been raised for that instance
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.2-servicing-10034");

            modelBuilder.Entity<DhtPackages>(entity =>
            {
                entity.HasKey(e => e.DhtPackageId);

                entity.Property(e => e.DhtPackageId).ValueGeneratedNever();

                entity.Property(e => e.Humidity).IsRequired();

                entity.Property(e => e.Received).IsRequired();

                entity.Property(e => e.Temperature).IsRequired();

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.DhtPackages)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).ValueGeneratedNever();

                entity.HasOne(d => d.ParentLocation)
                    .WithMany(p => p.InverseParentLocation)
                    .HasForeignKey(d => d.ParentLocationId);
            });
        }
    }
}
