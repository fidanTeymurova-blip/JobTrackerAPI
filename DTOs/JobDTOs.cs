namespace JobTrackerAPI.DTOs;

public record CreateJobDto(string Title, string? Description, string? Location, decimal? Salary, int CompanyId);
public record UpdateJobDto(string Title, string? Description, string? Location, decimal? Salary);