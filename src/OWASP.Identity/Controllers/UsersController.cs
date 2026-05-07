namespace OWASP.Identity.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OWASP.Application.Dtos;
using OWASP.Application.Interfaces;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController(IUserIdentityService service) : ControllerBase
{
    private readonly IUserIdentityService _service = service;

    //[HttpGet("profile")]
    //public async Task<ActionResult<ProfileResponse>> GetUserProfile([FromHeader] string token, [FromQuery] string userName)
    //{
    //    if (string.IsNullOrWhiteSpace(token)) return Unauthorized();

    //    var userToken = await _service.GetUserByToken(token);
    //    if (userToken is null) return Unauthorized();

    //    var user = await _service.GetUserByName(userName);

    //    if (user is null) return NotFound();

    //    var profile = await _service.GetProfile(user);

    //    return Ok(profile);
    //}

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.EmailAddress) || string.IsNullOrWhiteSpace(req.Password))
        {
            return BadRequest("Missing credentials.");
        }

        var token = await _service.Login(req.EmailAddress, req.Password);

        if (token is null)
        {
            return Unauthorized("Invalid credentials.");
        }

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
}
