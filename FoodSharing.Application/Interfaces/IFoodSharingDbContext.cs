using Microsoft.EntityFrameworkCore;
using NourishNet.Domain.Entities;
namespace FoodSharing.Application.Interfaces;


public interface IFoodSharingDbContext
{
    DbSet<Courier> Couriers { get; }
    DbSet<Beneficiary> Beneficiaries { get; }
    DbSet<Donor> Donors { get; }
    DbSet<Donation> Donations { get; }
    DbSet<DonationStatus> DonationStatuses { get; }
    DbSet<Order> Orders { get;  }
    DbSet<OrderStatus> OrderStatuses { get;  }
    DbSet<City> Cities { get;  }
    DbSet<Product> Products { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
