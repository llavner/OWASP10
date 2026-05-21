using System.ComponentModel.DataAnnotations;

namespace OWASP.Identity.Settings;

public class JwtSettings
{
    [Required]
    [MinLength(32)]
    public string Secret { get; set; } = string.Empty;

    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;
}
