using JobTrackerAPI.Data;
using JobTrackerAPI.DTOs;
using JobTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompaniesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CompaniesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Companies.Include(c => c.Jobs).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var company = await _context.Companies
            .Include(c => c.Jobs)
            .FirstOrDefaultAsync(c => c.Id == id);

        return company == null ? NotFound() : Ok(company);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCompanyDto dto)
    {
        var company = new Company
        {
            Name = dto.Name,
            Website = dto.Website,
            Industry = dto.Industry
        };

        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = company.Id }, company);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateCompanyDto dto)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null) return NotFound();

        company.Name = dto.Name;
        company.Website = dto.Website;
        company.Industry = dto.Industry;

        await _context.SaveChangesAsync();
        return Ok(company);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null) return NotFound();

        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}