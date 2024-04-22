using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NourishNet.Domain.Entities;
using NourishNet.Repository.Data;
using NourishNetAPI.DTO.Donation;

namespace NourishNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DonationController : ControllerBase
{
    private readonly FoodSharingDbContext _context;

    public DonationController(FoodSharingDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDonation([FromBody] CreateDonationDTO donationDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var donorExists = await _context.Donors.AnyAsync(d => d.Id == donationDTO.DonorId);
        if (!donorExists)
        {
            return NotFound($"Donor with ID {donationDTO.DonorId} not found.");
        }

        var donation = new Donation
        {
            DonorId = donationDTO.DonorId,
            ProductId  = donationDTO.ProductId,
            Quantity = donationDTO.Quantity,
            ExpirationDate = donationDTO.ExpirationDate,
            StatusId = donationDTO.StatusId
        };

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDonation), new { id = donation.Id }, donation);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DonationDetailDTO>> GetDonation(int id)
    {
        var donation = await _context.Donations
            .Include(d => d.Status)
            .Include(d => d.Product)
            .Where(d => d.Id == id)
            .Select(d=> new DonationDetailDTO
            {
                Id = d.Id,
                DonorId = d.DonorId,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId = d.StatusId,
                Status = d.Status.Name
            })
             .FirstOrDefaultAsync();
        if (donation == null)
        {
            return NotFound();
        }
        return donation;
    }

    [HttpGet("{cityId}")]
    public async Task<ActionResult<IList<DonationDetailDTO>>> GetDonationsByCityId(int cityId)
    {
        var donations = await _context.Donations
            .Include(d => d.Status)
            .Include(d => d.Donor)
            .Include(d => d.Product)
            .Where(d => d.Donor.CityId == cityId)

            .Select(d => new DonationDetailDTO
            {
                Id = d.Id,
                DonorId = d.DonorId,
                Product = d.Product.Name,
                Quantity = d.Quantity,
                ExpirationDate = d.ExpirationDate,
                StatusId = d.StatusId,
                Status = d.Status.Name
            })
            .ToListAsync();
        return donations;
    }

}
