using System.Text;

using Microsoft.IdentityModel.Tokens;

using OWASP.Application.Interfaces;
using OWASP.Application.Services;
using OWASP.Infrastructure.Repository;
using OWASP.Overtime.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultSecrets();
builder.AddOvertimeEntryDb();

builder.Services.AddScoped<IOvertimeEntryService, OvertimeEntryService>();
builder.Services.AddScoped<IOvertimeEntryRepository, OvertimeEntryRepository>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        };
    });

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
