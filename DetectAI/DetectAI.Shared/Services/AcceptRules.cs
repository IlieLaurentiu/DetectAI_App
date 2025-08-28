using System.Collections.Generic;
using System.Linq;


namespace DetectAI.Shared.Services
{
    public static class AcceptRules
    {
        private static readonly Dictionary<string, string> _acceptByCategory = new()
        {
            ["Image"] = "image/*,.jpg,.jpeg,.png,.webp,.bmp,.tiff",
            ["Video"] = "video/*,.mp4,.mov,.avi,.mkv,.webm",
            ["Audio"] = "audio/*,.mp3,.wav,.flac,.aac,.ogg",
            ["Text"] = "text/plain,.txt,.md" // expand only if you really support parsing them
        };


        public static string AcceptForMode(string? mode)
        {
            if (string.Equals(mode, "Media", System.StringComparison.OrdinalIgnoreCase))
            {
                var keys = new[] { "Image", "Video", "Audio" };
                var parts = keys.Where(_acceptByCategory.ContainsKey)
                .Select(k => _acceptByCategory[k])
                .SelectMany(s => s.Split(',', System.StringSplitOptions.RemoveEmptyEntries))
                .Select(s => s.Trim())
                .Distinct(System.StringComparer.OrdinalIgnoreCase);
                return string.Join(',', parts);
            }


            if (!string.IsNullOrWhiteSpace(mode) && _acceptByCategory.TryGetValue(mode!, out var accept))
                return accept;


            return string.Empty; // no filter
        }


        public static HashSet<string> AllowedExtensionsForMode(string? mode)
        {
            IEnumerable<string> parts = System.Array.Empty<string>();


            if (string.Equals(mode, "Media", System.StringComparison.OrdinalIgnoreCase))
            {
                var keys = new[] { "Image", "Video", "Audio" };
                parts = keys.Where(_acceptByCategory.ContainsKey)
                .SelectMany(k => _acceptByCategory[k].Split(',', System.StringSplitOptions.RemoveEmptyEntries));
            }
            else if (!string.IsNullOrWhiteSpace(mode) && _acceptByCategory.TryGetValue(mode!, out var accept))
            {
                parts = accept.Split(',', System.StringSplitOptions.RemoveEmptyEntries);
            }


            return parts.Select(p => p.Trim())
            .Where(p => p.StartsWith('.'))
            .Select(p => p.ToLowerInvariant())
            .ToHashSet();
        }
    }
}