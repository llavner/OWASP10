namespace OWASP.Identity.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Domain.Models;
using OWASP.Identity.Settings;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUserIdentityService service, IOptions<JwtSettings> jwtOptions) : ControllerBase
{
    private readonly IUserIdentityService _service = service;
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.EmailAddress) || string.IsNullOrWhiteSpace(req.Password))
        {
            return BadRequest("Missing credentials.");
        }

        var user = await _service.Login(req.EmailAddress, req.Password);

        if (user is null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var token = GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    [HttpGet("validate")]
    public async Task<ActionResult> ValidateToken([FromQuery] string token)
    {
        var user = await _service.GetUserByToken(token);

        if (user is null)
        {
            return NotFound();
        }

        var response = new ValidateRequest()
        {
            UserName = user.UserName,
            IsOnline = true,
        };

        return Ok(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest regReq)
    {
        var userEmail = await _service.GetUserByEmail(regReq.EmailAddress);
        var userName = await _service.GetUserByName(regReq.UserName);

        if (userEmail is not null)
        {
            return BadRequest($"{regReq.EmailAddress} already used.");
        }

        if (userName is not null)
        {
            return BadRequest($"{regReq.UserName} already used.");
        }

        await _service.Register(regReq);

        return Ok($"{regReq.UserName} successfully registered.");
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.id),
        new Claim(ClaimTypes.Name, user.UserName),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
