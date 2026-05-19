namespace OWASP.Overtime.Controllers;

using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OvertimeEntriesController(
    IOvertimeEntryService service,
    ILogger<OvertimeEntriesController> logger) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateEntry([FromBody] OvertimeEntryRequest request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("SecurityEvent: UnauthorizedRequest Path={Path}", HttpContext.Request.Path);
            return Unauthorized();
        }

        await service.AddEntryAsync(userId, request);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllForUser()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            logger.LogWarning("SecurityEvent: UnauthorizedRequest Path={Path}", HttpContext.Request.Path);
            return Unauthorized();
        }

        var entries = await service.GetAllEntriesAsync(userId);
        return Ok(entries);
    }
}
