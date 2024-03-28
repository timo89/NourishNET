using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NourishNet.Domain.Entities;
using NourishNet.Repository.Data;
using NourishNetAPI.DTO.Product;


[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly NourishNetDbContext _context;

    public ProductController(NourishNetDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public async Task<ActionResult<IList<ProductDTO>>> GetAllAsync()
    {
        var products = await _context.Products
            .Select(b => new ProductDTO
            {
                Id = b.Id,
                Name = b.Name,
            }).ToListAsync();

        return Ok(products);
    }

}
