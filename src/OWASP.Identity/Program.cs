using OWASP.Application.Interfaces;
using OWASP.Application.Services;
using OWASP.Identity;
using OWASP.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultSecrets();
builder.AddCosmosDb();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserIdentityService, UserIdentityService>();
builder.Services.AddScoped<IHashService, HashingService>();

builder.Services.AddScoped<IUserIdentityRepository, UserIdentityRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
