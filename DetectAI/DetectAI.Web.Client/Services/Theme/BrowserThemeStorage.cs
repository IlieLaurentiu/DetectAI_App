using DetectAI.Shared.Services.Theme;
using Microsoft.JSInterop;

namespace DetectAI.Web.Client.Services.Theme 
{
    public class BrowserThemeStorage : IThemeStorage
    {
        private readonly IJSRuntime _js;
        public BrowserThemeStorage(IJSRuntime js) => _js = js;

        public async Task<bool?> GetDarkModeAsync()
        {
            var val = await _js.InvokeAsync<string?>("localStorage.getItem", "isDarkMode");
            return bool.TryParse(val, out var b) ? b : null;
        }

        public Task SetDarkModeAsync(bool value) =>
            _js.InvokeVoidAsync("localStorage.setItem", "isDarkMode", value.ToString()).AsTask();
    }
}
