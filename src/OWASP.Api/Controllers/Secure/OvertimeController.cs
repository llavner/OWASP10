namespace OWASP.Api.Controllers.Secure;

using Microsoft.AspNetCore.Mvc;

[Route("api/secure/[controller]")]
[ApiController]
public class OvertimeController : ControllerBase
{
    // GET: api/<OvertimeController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<OvertimeController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<OvertimeController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<OvertimeController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<OvertimeController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
