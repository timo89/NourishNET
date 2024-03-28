using NourishNet.Domain.Entities;
using NourishNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NourishNetAPI.DTO.Donor;
using NourishNetAPI.DTO.Donation; // Ensure you have the corresponding DTO namespace

namespace NourishNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonorController : ControllerBase
{
    private readonly NourishNetDbContext _context;

    public DonorController(NourishNetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IList<DonorDTO>>> GetAllAsync()
    {
        var donors = await _context.Donors
            .Include(d => d.City)
            .Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                Address = d.Address,
                CityName = d.City.Name, // Assuming you also want to include the city name for donors
            }).ToListAsync();

        return Ok(donors);
    }

    [HttpGet("{id}")] // Note the use of a route parameter for specifying the donor ID
    public async Task<ActionResult<DonorDTO>> GetAsync(int id) // Removed nullable int since ID is essential
    {
        var donorDTO = await _context.Donors
            .Include(d=>d.Donations)
            .ThenInclude(donation => donation.Status)
            .Where(d => d.Id == id)
            .Select(d => new DonorDTO
            {
                Id = d.Id,
                Name = d.Name,
                Address = d.Address,
                CityName = d.City.Name,
                Donations = d.Donations.Select(donation => new DonationDTO
                {
                    Id = donation.Id,
                    Product = donation.Product.Name,
                    Quantity = donation.Quantity,
                    ExpirationDate = donation.ExpirationDate,
                    Status = donation.Status.Name
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (donorDTO == null)
        {
            return NotFound();
        }

        return Ok(donorDTO);
    }

    [HttpPost]
    public async Task<ActionResult<DonorDetailDTO>> CreateAsync([FromBody] CreateDonorDTO createDonorDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var donor = new Donor
        {
            Name = createDonorDTO.Name,
            Address = createDonorDTO.Address,
            CityId = createDonorDTO.CityId,
        };

        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();

        var donorDetailDTO = new DonorDetailDTO
        {
            Id = donor.Id,
            Name = donor.Name,
            Address = donor.Address,
            CityId = donor.CityId,
        };

        return Ok(donorDetailDTO);//
    }

    [HttpPut("{id}")] // Including the ID in the route to align with RESTful practices
    public async Task<IActionResult> EditAsync(int id, [FromBody] EditDonorDTO editDonorDTO)
    {
        if (id != editDonorDTO.Id)
        {
            return BadRequest("Mismatched Donor ID");
        }

        var donor = await _context.Donors.FindAsync(id);
        if (donor == null)
        {
            return NotFound();
        }

        // Map updated fields from EditDonorDTO to Donor
        donor.Name = editDonorDTO.Name;
        donor.Address = editDonorDTO.Address;
        donor.CityId = editDonorDTO.CityId;

        await _context.SaveChangesAsync();

        return NoContent(); 
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var donor = await _context.Donors.FindAsync(id);
        if (donor == null)
        {
            return NotFound();
        }

        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();
        return NoContent(); 
    }
}
