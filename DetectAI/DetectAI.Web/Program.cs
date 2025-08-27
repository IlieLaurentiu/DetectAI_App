using DetectAI.Shared.Services;
using DetectAI.Shared.Services.Theme;
using DetectAI.Web.Client.Services.Theme;
using DetectAI.Web.Components;
using DetectAI.Web.Services;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddMudServices();
// Add device-specific services used by the DetectAI.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<IThemeStorage, BrowserThemeStorage>();
builder.Services.AddScoped<DetectAI.Shared.Services.DetectionApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(DetectAI.Shared._Imports).Assembly,
        typeof(DetectAI.Web.Client._Imports).Assembly);

app.Run();
