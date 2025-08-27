namespace DetectAI.Shared.Services.Theme
{
    public interface IThemeStorage
    {
        Task<bool?> GetDarkModeAsync();
        Task SetDarkModeAsync(bool value);
    }
}
