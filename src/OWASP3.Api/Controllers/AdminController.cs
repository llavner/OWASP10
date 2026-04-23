namespace YourApp.Controllers;

using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public sealed class AdminController(ILogger<AdminController> logger) : ControllerBase
{
    private readonly ILogger<AdminController> _logger = logger;

    // A09 - Vulnerable: obehöriga försök loggas otydligt / utan kontext
    [Authorize] // kräver bara att man är "inloggad" (X-Demo-UserId)
    [HttpGet("/api/vulnerable/admin/panel")]
    public IActionResult VulnerableAdminPanel()
    {
        // Simulerad admin-check (policy hade varit mer "enterprise", men vi kör minimal logik här)
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin)
        {
            // VULNERABLE: för lite info för incidenthantering
            _logger.LogWarning("Access denied.");
            return Forbid();
        }

        return Ok(new { message = "Welcome to admin panel (vulnerable)." });
    }

    // A09 - Fixed: logga ett security event med kontext
    [Authorize] // fortfarande header-auth
    [HttpGet("/api/fixed/admin/panel")]
    public IActionResult FixedAdminPanel()
    {
        var isAdmin = User.IsInRole("Admin");
        if (!isAdmin)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "(unknown)";
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "(unknown)";
            var path = HttpContext.Request.Path.ToString();
            var traceId = HttpContext.TraceIdentifier;

            _logger.LogWarning(
                "SECURITY_EVENT forbidden_admin_access userId={UserId} ip={Ip} path={Path} traceId={TraceId}",
                userId,
                ip,
                path,
                traceId);

            return Forbid();
        }

        return Ok(new { message = "Welcome to admin panel (fixed)." });
    }
}
