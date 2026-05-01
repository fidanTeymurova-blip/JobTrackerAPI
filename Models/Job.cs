using static System.Net.Mime.MediaTypeNames;

namespace JobTrackerAPI.Models;

public class Job
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public decimal? Salary { get; set; }
    public DateTime PostedAt { get; set; } = DateTime.UtcNow;

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public ICollection<Application> Applications { get; set; } = new List<Application>();
}