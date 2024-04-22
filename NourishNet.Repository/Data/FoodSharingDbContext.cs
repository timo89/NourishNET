﻿using FoodSharing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using NourishNet.Domain.Entities;

namespace NourishNet.Repository.Data;

public class FoodSharingDbContext : DbContext, IFoodSharingDbContext
{
    public FoodSharingDbContext(DbContextOptions<FoodSharingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Beneficiary> Beneficiaries { get; set; }
    public DbSet<Donor> Donors { get; set; }
    public DbSet<Donation> Donations { get; set; }
    public DbSet<DonationStatus> DonationStatuses { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>().HasData(
            new City { Id = 1, Name = "Bucuresti" },
            new City { Id = 2, Name = "Cluj-Napoca" },
            new City { Id = 3, Name = "Timisoara" },
            new City { Id = 4, Name = "Iasi" },
            new City { Id = 5, Name = "Constanta" },
            new City { Id = 6, Name = "Craiova" },
            new City { Id = 7, Name = "Brasov" },
            new City { Id = 8, Name = "Galati" },
            new City { Id = 9, Name = "Ploiesti" },
            new City { Id = 10, Name = "Oradea" }
        );

        modelBuilder.Entity<Courier>().HasData(
            new Courier { Id = 1, Name = "DPD", Price=20 },
            new Courier { Id = 2, Name = "DHL", Price = 15 },
            new Courier { Id = 3, Name = "GLS", Price = 17.5M }
        );

        modelBuilder.Entity<DonationStatus>().HasData(
            new DonationStatus { Id = 1, Name = "Pending" },
            new DonationStatus { Id = 2, Name = "Approved" },
            new DonationStatus { Id = 3, Name = "Rejected" }
        );

        modelBuilder.Entity<OrderStatus>().HasData(
            new OrderStatus { Id = 1, Name = "Unconfirmed" },
            new OrderStatus { Id = 2, Name = "Confirmed" },
            new OrderStatus { Id = 3, Name = "InDelivery" },
            new OrderStatus { Id = 4, Name = "Delivered" }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "branza" },
            new Product { Id = 2, Name = "oua" },
            new Product { Id = 3, Name = "sunca" }
        );

        modelBuilder.Entity<Courier>()
            .Property(c => c.Price)
            .HasColumnType("decimal(18, 2)");

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Donation)
            .WithMany()
            .HasForeignKey(o => o.DonationId)
            .OnDelete(DeleteBehavior.Restrict); // Specify ON DE

        base.OnModelCreating(modelBuilder);
    }
}