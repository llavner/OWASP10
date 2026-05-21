using OWASP.Overtime.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultSecrets();
builder.AddOvertimeEntryDb();

builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

builder.Services.AddOvertimeEntryServices();

builder.Services.AddAuthenticationAndBearer(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
