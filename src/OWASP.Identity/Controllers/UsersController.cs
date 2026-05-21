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
public class UsersController(
    IUserIdentityService service,
    IOptions<JwtSettings> jwtOptions,
    ILogger<UsersController> logger) : ControllerBase
{
    private readonly IUserIdentityService _service = service;
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.EmailAddress) || string.IsNullOrWhiteSpace(req.Password))
        {
            logger.LogWarning(
                "SecurityEvent: LoginRejected_MissingCredentials Email={Email}",
                req.EmailAddress);

            return BadRequest("Missing credentials.");
        }

        var result = await _service.Login(req.EmailAddress, req.Password);

        if (!result.IsSuccess || result.Value is null)
        {
            logger.LogWarning(
                "SecurityEvent: LoginRejected Email={Email} Result={Result}",
                req.EmailAddress,
                result.Code);

            return Unauthorized(result.Error);
        }

        logger.LogInformation(
            "SecurityEvent: LoginSucceeded Email={Email} UserId={UserId} Role={Role}",
            result.Value.EmailAddress,
            result.Value.id,
            result.Value.Role ?? "User");

        var token = GenerateJwtToken(result.Value);
        return Ok(new { Token = token });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest regReq)
    {
        var emailResult = await _service.GetUserByEmail(regReq.EmailAddress);
        var nameResult = await _service.GetUserByName(regReq.UserName);

        if (emailResult.IsSuccess && emailResult.Value is not null)
        {
            logger.LogWarning(
                "SecurityEvent: RegisterRejected_DuplicateEmail Email={Email}",
                regReq.EmailAddress);

            return BadRequest($"{regReq.EmailAddress} already used.");
        }

        if (nameResult.IsSuccess && nameResult.Value is not null)
        {
            logger.LogWarning(
                "SecurityEvent: RegisterRejected_DuplicateUsername UserName={UserName}",
                regReq.UserName);

            return BadRequest($"{regReq.UserName} already used.");
        }

        var registerResult = await _service.Register(regReq);

        if (!registerResult.IsSuccess)
        {
            logger.LogWarning(
                "SecurityEvent: RegisterRejected Email={Email} UserName={UserName} Result={Result}",
                regReq.EmailAddress,
                regReq.UserName,
                registerResult.Code);

            return BadRequest(registerResult.Error);
        }

        logger.LogInformation(
            "SecurityEvent: RegisterSucceeded Email={Email} UserName={UserName}",
            regReq.EmailAddress,
            regReq.UserName);

        return Ok();
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.id),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Email, user.EmailAddress),
        new Claim(ClaimTypes.Role, user.Role ?? "User"),
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
