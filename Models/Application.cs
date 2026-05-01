namespace JobTrackerAPI.Models;

public enum ApplicationStatus
{
    Applied,
    Interview,
    Offer,
    Rejected
}

public class Application
{
    public int Id { get; set; }
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public string? Notes { get; set; }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
}