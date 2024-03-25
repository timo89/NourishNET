using NourishNet.Domain.Entities;
using NourishNet.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NourishNetAPI.DTO.Beneficiary;

namespace NourishNetAPI.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BeneficiaryController : ControllerBase
{
    private readonly NourishNetDbContext _context;

    public BeneficiaryController(NourishNetDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IList<BeneficiaryDTO>>> GetAllAsync()
    {
        var beneficiaries = await _context.Beneficiaries
            .Include(b => b.City)
            .Select(b => new BeneficiaryDTO
            {
                Id = b.Id,
                Name = b.Name,
                Address = b.Address,
                CityName = b.City.Name,
            }).ToListAsync();

        return Ok(beneficiaries);
    }

    [HttpGet]
    public async Task<ActionResult<BeneficiaryDTO>> GetAsync(int? id)
    {
        var beneficiaryDTO = await _context.Beneficiaries
             .Select(b => new BeneficiaryDTO
             {
                 Id = b.Id,
                 Name = b.Name,
                 Address = b.Address,
                 CityName = b.City.Name,
             })
            .FirstOrDefaultAsync(m => m.Id == id);

        if (beneficiaryDTO == null)
        {
            return NotFound();
        }

        return Ok(beneficiaryDTO);
    }

    [HttpPost]
    public async Task<ActionResult<BeneficiaryDetailDTO>> CreateAsync(CreateBeneficiaryDTO createBeneficiaryDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var beneficiary = new Beneficiary
        {
            Name = createBeneficiaryDTO.Name,
            Address = createBeneficiaryDTO.Address,
            CityId = createBeneficiaryDTO.CityId,
            Capacity = createBeneficiaryDTO.Capacity
        };

        _context.Add(beneficiary);
        await _context.SaveChangesAsync();

        var beneficiaryEntityDTO = new BeneficiaryDetailDTO
        {
            Id = beneficiary.Id,
            Name = beneficiary.Name,
            Address = beneficiary.Address,
            CityId = beneficiary.CityId,
            Capacity = beneficiary.Capacity
        };

        return Ok(beneficiaryEntityDTO);
    }

    [HttpPut]
    public async Task<IActionResult> EditAsync(int id, EditBeneficiaryDTO editBeneficiaryDTO)
    {
        if (id != editBeneficiaryDTO.Id)
        {
            return BadRequest("Mismatched Beneficiary ID");
        }

        var beneficiary = await _context.Beneficiaries
            .FirstOrDefaultAsync(b => b.Id == editBeneficiaryDTO.Id);

        if (beneficiary == null)
        {
            return NotFound();
        }

        beneficiary.Name = editBeneficiaryDTO.Name;
        beneficiary.Address = editBeneficiaryDTO.Address;
        beneficiary.CityId = editBeneficiaryDTO.CityId;
        beneficiary.Capacity = editBeneficiaryDTO.Capacity;


        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var beneficiary = await _context.Beneficiaries.FindAsync(id);

        if (beneficiary == null)
        {
            return NotFound();
        }

        _context.Beneficiaries.Remove(beneficiary);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
