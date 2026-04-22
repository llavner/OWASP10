namespace OWASP.Application.Handlers;

using System.Security.Claims;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class DemoAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "DemoHeader";

    public DemoAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 1) Läs user id från header
        if (!Request.Headers.TryGetValue("X-Demo-UserId", out var userIdValues))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing X-Demo-UserId header."));
        }

        var userId = userIdValues.ToString().Trim();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Task.FromResult(AuthenticateResult.Fail("Empty X-Demo-UserId header."));
        }

        // 2) (Valfritt men bra) Roll från header för A09 admin-demo
        var role = Request.Headers.TryGetValue("X-Demo-Role", out var roleValues)
            ? roleValues.ToString().Trim()
            : null;

        // 3) Skapa claims som i en riktig app (NameIdentifier används ofta som user id)
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, userId),
        };

        if (!string.IsNullOrWhiteSpace(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
