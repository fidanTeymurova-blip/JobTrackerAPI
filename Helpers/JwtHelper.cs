using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobTrackerAPI.Models;
using Microsoft.IdentityModel.Tokens;
using JobTrackerAPI.DTOs;

namespace JobTrackerAPI.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _config;

    public JwtHelper(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: _config["JwtSettings:Issuer"],
            audience: _config["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                int.Parse(_config["JwtSettings:ExpirationHours"]!)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public static class ApplicationMapper
{
    public static ApplicationDto ToDto(Application app)
    {
        return new ApplicationDto(
            app.Id,
            app.Job.Title,
            app.Job.Company.Name,
            app.Status.ToString(),
            app.Notes,
            app.AppliedAt
        );
    }
}
}