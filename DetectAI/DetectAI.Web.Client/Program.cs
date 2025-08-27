using DetectAI.Shared.Services;
using DetectAI.Shared.Services.Theme;
using DetectAI.Web.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add device-specific services used by the DetectAI.Shared project
builder.Services.AddSingleton<IFormFactor, FormFactor>();
builder.Services.AddScoped<IThemeService, ThemeService>();
builder.Services.AddScoped<DetectAI.Shared.Services.DetectionApiClient>();

await builder.Build().RunAsync();
