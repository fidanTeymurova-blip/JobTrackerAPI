using System.Security.Claims;
using JobTrackerAPI.Data;
using JobTrackerAPI.DTOs;
using JobTrackerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JobTrackerAPI.Helpers;

namespace JobTrackerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ApplicationsController(AppDbContext context)
    {
        _context = context;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMyApplications()
    {
        var userId = GetUserId();

        var apps = await _context.Applications
            .Where(a => a.UserId == userId)
            .Include(a => a.Job)
            .ThenInclude(j => j.Company)
            .ToListAsync();

        var result = apps.Select(ApplicationMapper.ToDto);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Apply(CreateApplicationDto dto)
    {
        var userId = GetUserId();

        if (!await _context.Jobs.AnyAsync(j => j.Id == dto.JobId))
            return BadRequest("İş elanı tapılmadı.");

        if (await _context.Applications.AnyAsync(
            a => a.UserId == userId && a.JobId == dto.JobId))
            return BadRequest("Bu işə artıq müraciət etmisiniz.");

        var application = new Application
        {
            UserId = userId,
            JobId = dto.JobId,
            Notes = dto.Notes
        };

        _context.Applications.Add(application);
        await _context.SaveChangesAsync();
        return Ok(ApplicationMapper.ToDto(application));
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateApplicationDto dto)
    {
        var userId = GetUserId();

        var app = await _context.Applications
            .Include(x => x.Job)
            .ThenInclude(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);

        if (app == null)
            return NotFound();

        // UPDATE logic
        app.Status = dto.Status;
        app.Notes = dto.Notes;

        await _context.SaveChangesAsync();

        var result = ApplicationMapper.ToDto(app);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var app = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

        if (app == null) return NotFound();

        _context.Applications.Remove(app);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}