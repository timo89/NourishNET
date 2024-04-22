using FoodSharing.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NourishNet.Domain.Entities;
using NourishNetAPI.DTO.Order;
using OrderStatusEnum = NourishNet.Domain.Enums.OrderStatus;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // POST: api/Order
    [HttpPost]
    public async Task<ActionResult<OrderDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
    {
        var order = new Order
        {
            Quantity = createOrderDTO.Quantity,
            BeneficiaryId = createOrderDTO.BeneficiaryId,
            DonationId = createOrderDTO.DonationId,
            CourierId = createOrderDTO.CourierId,
            CreationDate = createOrderDTO.CreationDate,
            OrderStatusId = createOrderDTO.OrderStatusId
            
        };

        var createdOrder = await _orderService.CreateOrderAsync(order);

        var orderDetails = new OrderDetailsDTO
        {
            Id = createdOrder.Id,
            BeneficiaryId = createdOrder.BeneficiaryId,
            DonationId = createdOrder.DonationId,
            CourierId = createdOrder.CourierId,
            CreationDate = createdOrder.CreationDate,
            DeliveryDate = createdOrder.DeliveryDate,
            OrderStatusId = createdOrder.OrderStatusId,
            Quantity = createdOrder.Quantity,
        };

        return Ok(orderDetails);
    }

    // GET: api/Order/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetOrder(int id)
    {
        var order = await _orderService.GetOrderAsync(id);

        var orderDto = new OrderDTO
        {
            Id = order.Id,
            BeneficiaryId = order.BeneficiaryId,
            BeneficiaryName = order.Beneficiary.Name,
            DonationId = order.DonationId,
            DonationProduct = order.Donation.Product.Name,
            CourierId = order.CourierId,
            CourierName = order.Courier.Name,
            CreationDate = order.CreationDate,
            DeliveryDate = order.DeliveryDate,
            OrderStatusId = order.OrderStatusId,
            OrderStatusName = order.OrderStatus.Name
        };

        return Ok(orderDto);
    }

    [HttpPatch("{orderId:int}/status")] 
    public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO updateStatusDTO)
    {
        if (orderId != updateStatusDTO.OrderId)
        {
            return BadRequest("Mismatched Order ID");
        }

        await _orderService.UpdateOrderStatusAsync(orderId, (OrderStatusEnum)updateStatusDTO.NewStatusId);

        return NoContent(); 
    }
}
