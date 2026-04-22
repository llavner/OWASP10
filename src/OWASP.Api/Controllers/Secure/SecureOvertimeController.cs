namespace OWASP.Api.Controllers.Secure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;
using OWASP.Application.Services;

[Route("api/secure/[controller]")]
[ApiController]
[AllowAnonymous]
public class SecureOvertimeController : ControllerBase
{
    private readonly SecureOvertimeEntryService _service;
    private readonly ICurrentUserAccessor _currentUser;

    public SecureOvertimeController(SecureOvertimeEntryService service, ICurrentUserAccessor currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OvertimeEntryResponse>>> Get(Guid userId)
    {
        var entries = _service.ReadAllEntries(userId);

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OvertimeEntryResponse>> Get(OvertimeEntryResponse req)
    {
        var entry = _service.ReadEntry(req);
        return Ok(entry);
    }

    [HttpPost]
    public async Task<ActionResult> Post([FromBody] OvertimeEntryCreate entry)
    {
        await _service.CreateEntry(entry);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Put([FromBody] OvertimeEntryUpdate entry)
    {
        await _service.UpdateEntry(entry);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _service.DeleteEntry(id, _currentUser.GetUserId());
        return Ok();
    }
}
