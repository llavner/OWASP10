namespace OWASP.Application.Dtos;

using System.ComponentModel.DataAnnotations;

public class RegisterRequest
{
    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [RegularExpression(
        @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{6,}$",
        ErrorMessage = "Password must be at least 6 characters long and contain an uppercase letter, a lowercase letter, and a number.")]
    public string Password { get; set; } = string.Empty;
}
