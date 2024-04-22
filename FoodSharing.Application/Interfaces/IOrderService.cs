using NourishNet.Domain.Entities;
using Enums = NourishNet.Domain.Enums;

namespace FoodSharing.Application.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order);

    Task<Order> GetOrderAsync(int id);

    Task<bool> UpdateOrderStatusAsync(int orderId, Enums.OrderStatus orderStatus);
}
