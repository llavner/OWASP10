namespace OWASP.Application.Dtos;

public class ValidateRequest
{
    public string UserName { get; set; } = string.Empty;

    public bool IsOnline { get; set; }
}
