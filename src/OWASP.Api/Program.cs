using OWASP.Api;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultSecrets();
builder.AddCosmosDb();

builder.Services.AddHealthChecks();
builder.Services.AddApiRepositories();

builder.Services.AddSecureServices();
builder.Services.AddInsecureServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();
