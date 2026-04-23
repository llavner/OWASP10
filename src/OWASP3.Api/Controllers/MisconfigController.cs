namespace OWASP3.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public sealed class MisconfigController : ControllerBase
{
    [Authorize]
    [HttpGet("/api/vulnerable/misconfig/read-missing-file")]
    public async Task<IActionResult> ReadMissingFile()
    {
        // Detta kastar en riktig FileNotFoundException / DirectoryNotFoundException
        var text = await System.IO.File.ReadAllTextAsync("secrets/appsettings.Production.json");
        return Ok(new { text });
    }

    [Authorize]
    [HttpGet("/api/fixed/misconfig/read-missing-file")]
    public async Task<IActionResult> ReadMissingFileFixed()
    {
        try
        {
            var text = await System.IO.File.ReadAllTextAsync("secrets/appsettings.Production.json");
            return Ok(new { text });
        }
        catch (Exception ex)
        {
            // Logga internt, men läck inte detaljer till klient
            HttpContext.RequestServices
                .GetRequiredService<ILogger<MisconfigController>>()
                .LogError(ex, "Failed to read file. traceId={TraceId}", HttpContext.TraceIdentifier);

            return Problem(
                title: "An error occurred.",
                statusCode: StatusCodes.Status500InternalServerError,
                detail: "Something went wrong.",
                instance: HttpContext.TraceIdentifier);
        }
    }
}
