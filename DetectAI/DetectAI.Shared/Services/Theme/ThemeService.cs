using MudBlazor;

namespace DetectAI.Shared.Services.Theme
{
    public class ThemeService : IThemeService
    {
        public MudTheme Theme { get; } = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = Colors.Indigo.Default,
                Secondary = Colors.Pink.Default,
                AppbarBackground = Colors.Gray.Lighten5
            },
            PaletteDark = new PaletteDark()
            {
                Primary = Colors.Indigo.Lighten1,
                Secondary = Colors.Pink.Lighten1,
                Background = Colors.Gray.Darken4,
                Surface = Colors.Gray.Darken3
            }
        };

        public bool IsDarkMode { get; private set; }
        public event Action? Changed;

        private readonly IThemeStorage _storage;

        public ThemeService(IThemeStorage storage) => _storage = storage;

        public async Task InitializeAsync(MudThemeProvider provider)
        {
            // 1) Load saved preference (if any)
            var saved = await _storage.GetDarkModeAsync();
            if (saved.HasValue)
            {
                IsDarkMode = saved.Value;
            }
            else
            {
                // 2) Otherwise follow system preference
                var system = await provider.GetSystemDarkModeAsync();
                IsDarkMode = system;

                // Optional: keep following OS changes
                await provider.WatchSystemDarkModeAsync(async (newPreference) =>
                {
                    var currentSaved = await _storage.GetDarkModeAsync();
                    if (!currentSaved.HasValue) // only if user hasn't chosen manually
                    {
                        IsDarkMode = newPreference;
                        Changed?.Invoke();
                    }
                });
            }

            Changed?.Invoke();
        }

        public async Task SetDarkModeAsync(bool value)
        {
            IsDarkMode = value;
            await _storage.SetDarkModeAsync(value);
            Changed?.Invoke();
        }

        public Task ToggleAsync() => SetDarkModeAsync(!IsDarkMode);
    }
}