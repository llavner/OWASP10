using OWASP.Application.Factories;
using OWASP.Application.Interfaces;
using OWASP.Application.Services;
using OWASP.Identity.Settings;
using OWASP.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultSecrets();
builder.AddIdentityDb();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

builder.Services.AddTransient<IUserIdentityFactory, UserIdentityFactory>();

builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();
builder.Services.AddScoped<IHashService, HashingService>();

builder.Services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
