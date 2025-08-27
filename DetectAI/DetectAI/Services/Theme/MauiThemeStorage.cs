using DetectAI.Shared.Services.Theme;

namespace DetectAI.Services.Theme
{
    public class MauiThemeStorage : IThemeStorage
    {
        public Task<bool?> GetDarkModeAsync()
        {
            if (!Preferences.Default.ContainsKey("isDarkMode"))
                return Task.FromResult<bool?>(null);
            return Task.FromResult<bool?>(Preferences.Default.Get("isDarkMode", false));
        }

        public Task SetDarkModeAsync(bool value)
        {
            Preferences.Default.Set("isDarkMode", value);
            return Task.CompletedTask;
        }
    }
}
