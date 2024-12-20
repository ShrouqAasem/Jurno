﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Travel.Models
{
    public class TravelDbContext : IdentityDbContext<ApplicationUser>
    {
        public TravelDbContext(DbContextOptions<TravelDbContext> options)
            : base(options)
        {
        }
    
        public DbSet<Tour> Tours{ get; set; }

        //public DbSet<User> Users { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<TravelGuide> Destinations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // User-Booking relationship
            modelBuilder.Entity<Booking>()
            .HasOne(b => b.User)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.UserId);

            // TravelPackage-Booking relationship
            modelBuilder.Entity<Booking>()
            .HasOne(b => b.Tour)
            .WithMany(tp => tp.Bookings)
            .HasForeignKey(b => b.TourId);

            modelBuilder.Entity<Tour>()
              .Property(t => t.Price)
              .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Tour>()
                .Property(t => t.Discount)
                .HasColumnType("decimal(5, 2)");

        }
    }
}

    
