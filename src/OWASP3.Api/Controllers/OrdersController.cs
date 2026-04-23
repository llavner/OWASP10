namespace OWASP3.Api.Controllers;

using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OWASP.Infrastructure.Repository;

[Route("api/[controller]")]
[ApiController]
public sealed class OrdersController(InMemoryStore store) : ControllerBase
{
    private readonly InMemoryStore _store = store;

    // A01 - Vulnerable: IDOR (ingen ägarskapskontroll)
    [Authorize]
    [HttpGet("/api/vulnerable/orders/{orderId:int}")]
    public IActionResult GetVulnerable(int orderId)
    {
        if (!_store.Orders.TryGetValue(orderId, out var order))
        {
            return NotFound();
        }

        // VULNERABLE: returnerar alltid ordern, oavsett vem som äger den
        return Ok(order);
    }

    // A01 - Fixed: object-level authorization (ägarskapskontroll)
    [Authorize]
    [HttpGet("/api/fixed/orders/{orderId:int}")]
    public IActionResult GetFixed(int orderId)
    {
        if (!_store.Orders.TryGetValue(orderId, out var order))
        {
            return NotFound();
        }

        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            return Unauthorized(); // borde inte hända om auth funkar, men bra guard
        }

        if (!string.Equals(order.UserId, currentUserId, StringComparison.Ordinal))
        {
            return Forbid(); // alternativ: NotFound() för att inte “läcka existens”
        }

        return Ok(order);
    }
}
