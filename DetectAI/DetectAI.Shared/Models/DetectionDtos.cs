namespace DetectAI.Shared.Models;

public enum MediaType { Image, Video, Audio }

public sealed class AnalyzeResult
{
    public string FileName { get; set; } = "";
    public MediaType MediaType { get; set; }
    public float CombinedScore { get; set; } // 0..1 (higher == more likely AI)
    public string Summary { get; set; } = "";
    public DateTimeOffset AnalyzedAt { get; set; } = DateTimeOffset.UtcNow;
    public Dictionary<string, object>? Raw { get; set; }
}