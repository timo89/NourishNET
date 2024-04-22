using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NourishNet.Domain.Entities;
using NourishNet.Repository.Data;
using NourishNetAPI.DTO.Beneficiary;
using NourishNetAPI.DTO.Order;
using OrderStatusEnum = NourishNet.Domain.Enums.OrderStatus;

[Route("api/[controller]")]
[ApiController]
public class CourierController : ControllerBase
{
    private readonly FoodSharingDbContext _context;

    public CourierController(FoodSharingDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IList<CourierDTO>>> GetAllAsync()
    {
        var couriers = await _context.Couriers
            .Select(b => new CourierDTO
            {
                Id = b.Id,
                Name = b.Name,
                Price = b.Price
            }).ToListAsync();

        return Ok(couriers);
    }

}
