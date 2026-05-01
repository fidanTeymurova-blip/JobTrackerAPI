using JobTrackerAPI.DTOs;
using JobTrackerAPI.Models;

namespace JobTrackerAPI.Helpers;

public static class ApplicationMapper
{
    public static ApplicationDto ToDto(Application app)
    {
        return new ApplicationDto(
            app.Id,
            app.Job?.Title ?? "",
            app.Job?.Company?.Name ?? "",
            app.Status.ToString(),
            app.Notes,
            app.AppliedAt
        );
    }
}