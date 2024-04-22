using FoodSharing.Application.Exceptions;
using FoodSharing.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using NourishNet.Domain.Entities;
using Enums = NourishNet.Domain.Enums;

namespace FoodSharing.Application.Services;

public class OrderService : IOrderService
{
    private readonly IFoodSharingDbContext _context;

    public OrderService(IFoodSharingDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Order> CreateOrderAsync(Order order, int quantity)
    {
        var donation = await _context.Donations
            .FirstOrDefaultAsync(d => d.Id == order.DonationId);

        // Check if the donation exists
        if (donation == null)
        {
            throw new NotFoundException("Donation", order.DonationId);
        }

        // Check if the requested quantity is available
        if (donation.Quantity < quantity)
        {
            throw new OrderException($"Requested quantity exceeds available quantity for Donation ID {order.DonationId}.");
        }

        donation.Quantity -= quantity;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order> GetOrderAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Beneficiary)
            .Include(o => o.Donation)
            .Include(o => o.Donation.Product)
            .Include(o => o.Courier)
            .Include(o => o.OrderStatus)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            throw new NotFoundException("Order", id);
        }

        return order;
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, Enums.OrderStatus orderStatus)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            throw new NotFoundException("Order", orderId);
        }

        order.OrderStatusId = (int)orderStatus;

        order.DeliveryDate = orderStatus == Enums.OrderStatus.Delivered ? DateTime.UtcNow : order.DeliveryDate;

        await _context.SaveChangesAsync();

        return true;
    }
}
