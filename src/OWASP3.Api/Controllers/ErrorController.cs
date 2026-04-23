namespace OWASP3.Api.Controllers;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public sealed class ErrorController(ILogger<ErrorController> logger) : ControllerBase
{
    private readonly ILogger<ErrorController> _logger = logger;

    [HttpGet("/error")]
    public IActionResult Handle()
    {
        var ex = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        _logger.LogError(ex, "Unhandled exception. traceId={TraceId}", HttpContext.TraceIdentifier);

        return Problem(
            title: "An error occurred.",
            statusCode: StatusCodes.Status500InternalServerError,
            detail: "Something went wrong.",
            instance: HttpContext.TraceIdentifier);
    }
}
