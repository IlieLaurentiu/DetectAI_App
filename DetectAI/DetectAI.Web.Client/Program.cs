using DetectAI.Shared.Models;
using DetectAI.Shared.Services;
using DetectAI.Shared.Services.Theme;
using DetectAI.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the DetectAI.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<DetectAIApiClient>();
builder.Services.AddScoped<ForensicsState>();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://127.0.0.1:8000") // your API base
});

await builder.Build().RunAsync();
