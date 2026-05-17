using System.Text;

using Microsoft.IdentityModel.Tokens;

using OWASP.Application.Interfaces;
using OWASP.Application.Services;
using OWASP.Infrastructure.Repository;
using OWASP.Overtime;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultSecrets();
builder.AddOvertimeEntryDb();

builder.Services.AddScoped<IOvertimeEntryService, OvertimeEntryService>();
builder.Services.AddScoped<IOvertimeEntryRepository, OvertimeEntryRepository>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "YourIssuer",
            ValidAudience = "YourAudience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperSecretKey")),
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
