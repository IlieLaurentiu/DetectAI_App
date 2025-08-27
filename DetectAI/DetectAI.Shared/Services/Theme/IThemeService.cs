using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectAI.Shared.Services.Theme
{
    public interface IThemeService
    {
        MudTheme Theme { get; }
        bool IsDarkMode { get; }
        event Action? Changed;

        Task InitializeAsync(MudThemeProvider provider);
        Task SetDarkModeAsync(bool value);
        Task ToggleAsync();
    }

}
