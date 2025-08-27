using DetectAI.Maui.Services.Theme;
using DetectAI.Services;
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
            builder.Services.AddScoped<IThemeService, ThemeService>();
            builder.Services.AddSingleton<IThemeStorage, MauiThemeStorage>();

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
