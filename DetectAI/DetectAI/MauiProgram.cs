using DetectAI.Services;
using DetectAI.Services.Theme;
using DetectAI.Shared.Models;
using DetectAI.Shared.Services;
using DetectAI.Shared.Services.Theme;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace DetectAI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddMudServices();
            builder.Services.AddSingleton<IThemeService, ThemeService>();
            builder.Services.AddSingleton<IThemeStorage, MauiThemeStorage>();
            builder.Services.AddSingleton<DetectAIApiClient>();
            builder.Services.AddSingleton(sp => new HttpClient
            {
                BaseAddress = new Uri("http://127.0.0.1:8000") // your API base
            });
            builder.Services.AddSingleton<ForensicsState>();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            // Add device-specific services used by the DetectAI.Shared project
            builder.Services.AddSingleton<IFormFactor, FormFactor>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
