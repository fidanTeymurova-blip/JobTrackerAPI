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
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _context.Jobs.Include(j => j.Company).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var job = await _context.Jobs
            .Include(j => j.Company)
            .FirstOrDefaultAsync(j => j.Id == id);

        return job == null ? NotFound() : Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateJobDto dto)
    {
        if (!await _context.Companies.AnyAsync(c => c.Id == dto.CompanyId))
            return BadRequest("Şirkət tapılmadı.");

        var job = new Job
        {
            Title = dto.Title,
            Description = dto.Description,
            Location = dto.Location,
            Salary = dto.Salary,
            CompanyId = dto.CompanyId
        };

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateJobDto dto)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return NotFound();

        job.Title = dto.Title;
        job.Description = dto.Description;
        job.Location = dto.Location;
        job.Salary = dto.Salary;

        await _context.SaveChangesAsync();
        return Ok(job);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job == null) return NotFound();

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}