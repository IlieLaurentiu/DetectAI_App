using DetectAI.Shared.Models;
using DetectAI.Shared.Services;
using DetectAI.Shared.Services.Theme;
using DetectAI.Shared.Validators;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DetectAI.Shared.Pages
{
    public partial class Detect : ComponentBase, IDisposable
    {
        private readonly UploadModel _model = new();
        private readonly UploadModelValidator _validator = new();


        private Components.FileDropZone? _dropZone;


        private bool _isValid;
        private bool _isTouched;
        private string? _detectionMode = "Media";
        private bool hasUploadedFiles = false; // drives the "Analyzing" UI
        private bool isUploading = false; // disables button / double submits
        private bool _dark;
        private string? TextFieldString { get; set; }


        [Inject] public IThemeService ThemeSvc { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager Nav { get; set; } = default!;
        [Inject] public ForensicsState State { get; set; } = default!;
        [Inject] public DetectAIApiClient ApiClient { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            _dark = ThemeSvc.IsDarkMode;
            ThemeSvc.Changed += OnThemeChanged;


            var ok = await ApiClient.PingAsync();
            if (!ok)
                Snackbar.Add("API not reachable or did not return pong.", Severity.Error);
        }


        private void OnThemeChanged()
        {
            _dark = ThemeSvc.IsDarkMode;
            InvokeAsync(StateHasChanged);
        }


        private void OnFilesChanged(IReadOnlyList<IBrowserFile>? files)
        {
            files ??= Array.Empty<IBrowserFile>();


            var allowed = AcceptRules.AllowedExtensionsForMode(_detectionMode);


            if (allowed.Count == 0)
            {
                _model.Files = files;
                _isTouched = true;
                StateHasChanged();
                return;
            }


            var valid = files
            .Where(f => f is not null)
            .Where(f => allowed.Contains(Path.GetExtension(f.Name).ToLowerInvariant()))
            .ToList();


            if (valid.Count != files.Count)
            {
                var removed = files.Except(valid).Select(f => f.Name);
                Snackbar.Add($"Removed unsupported files: {string.Join(", ", removed)}", Severity.Warning);
            }


            _model.Files = valid;
            _isTouched = true;
            StateHasChanged();
        }

        private async Task Upload()
        {
            if (_model.Files is null || !_model.Files.Any()) return;


            var allowed = AcceptRules.AllowedExtensionsForMode(_detectionMode);
            if (allowed.Count > 0)
            {
                var bad = _model.Files.Where(f => !allowed.Contains(Path.GetExtension(f.Name).ToLowerInvariant()))
                .Select(f => f.Name).ToList();
                if (bad.Count > 0)
                {
                    Snackbar.Add($"These files are not allowed: {string.Join(", ", bad)}", Severity.Warning);
                    return;
                }
            }


            var file = _model.Files.First();


            isUploading = true;
            hasUploadedFiles = true;
            StateHasChanged();


            try
            {
                Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
                Snackbar.Add($"Uploading {_model.Files.Count} file(s) for {_detectionMode}...", Severity.Info);

                State.LastFileName = file.Name;
                State.LastFileContentType = file.ContentType;
                State.LastFileDataUrl = await ToDataUrlAsync(file);

                var result = await ApiClient.AnalyzeAsync(file);
                State.LastResult = result;
                Nav.NavigateTo("/AnalysisResults");
            }
            catch (Exception ex)
            {
                hasUploadedFiles = false;
                Snackbar.Add($"Upload failed: {ex.Message}", Severity.Error);
                StateHasChanged();
            }
            finally
            {
                isUploading = false;
            }
        }
        private static async Task<string> ToDataUrlAsync(IBrowserFile file)
        {
            using var s = file.OpenReadStream(200 * 1024 * 1024);
            using var ms = new MemoryStream();
            await s.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());
            var mime = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            return $"data:{mime};base64,{base64}";
        }

        private async Task OnModeChanged(string _)
        {
            _model.Files = Array.Empty<IBrowserFile>();
            _isValid = _isTouched = false;
            hasUploadedFiles = false;


            if (_dropZone is not null)
            {
                try { await _dropZone.ClearAsync(); } catch { }
            }


            StateHasChanged();
        }
        private Task OpenFilePickerAsync() => _dropZone?.OpenFilePickerAsync() ?? Task.CompletedTask;
        private Task ClearAsync() => _dropZone?.ClearAsync() ?? Task.CompletedTask;


        public void Dispose()
        {
            ThemeSvc.Changed -= OnThemeChanged;
        }
    }
}