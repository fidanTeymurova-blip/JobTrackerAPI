using JobTrackerAPI.Models;

namespace JobTrackerAPI.DTOs;

public record CreateApplicationDto(int JobId, string? Notes);
public record UpdateApplicationDto(ApplicationStatus Status, string? Notes);
public record ApplicationDto
(
    int Id,
    string JobTitle,
    string CompanyName,
    string Status,
    string? Notes,
    DateTime AppliedAt
);