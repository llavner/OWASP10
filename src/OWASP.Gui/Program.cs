using MudBlazor.Services;

using OWASP.Gui;
using OWASP.Gui.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddKeyVaultSecrets();
builder.AddCosmosDb();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
