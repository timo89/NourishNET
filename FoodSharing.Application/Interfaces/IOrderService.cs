using NourishNet.Domain.Entities;
using Enums = NourishNet.Domain.Enums;

namespace FoodSharing.Application.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order, int quantity);

    Task<Order> GetOrderAsync(int id);

    Task<bool> UpdateOrderStatusAsync(int orderId, Enums.OrderStatus orderStatus);
}
