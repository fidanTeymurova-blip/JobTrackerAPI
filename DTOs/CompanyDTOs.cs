namespace JobTrackerAPI.DTOs;

public record CreateCompanyDto(string Name, string? Website, string? Industry);
public record UpdateCompanyDto(string Name, string? Website, string? Industry);