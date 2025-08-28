namespace DetectAI.Shared.Models
{
    public sealed class ForensicsResult
    {
        public string? path { get; set; }
        public string? type { get; set; }  // "image" | "video" | "audio"
        public double? combined_score { get; set; }
        public ImageMetrics? image { get; set; }
        public VideoMetrics? video { get; set; }
        public AudioMetrics? audio { get; set; }
        public string? error { get; set; }
    }
    public sealed class ImageMetrics
    {
        public bool? exif_present { get; set; }
        public double? exif_score { get; set; }
        public double? jpeg_q_score { get; set; }
        public double? resid_std { get; set; }
        public double? resid_skew { get; set; }
        public double? resid_kurt { get; set; }
        public double? residual_std_score { get; set; }
        public double? hf_ratio { get; set; }
        public double? hf_score { get; set; }
    }
    public sealed class FrameMetrics
    {
        public double? resid_std { get; set; }
        public double? hf_ratio { get; set; }
        public double? combined_score { get; set; }
    }
    public sealed class VideoMetrics
    {
        public double? frame_scores_mean { get; set; }
        public double? frame_scores_std { get; set; }
        public double? hf_mean { get; set; }
        public double? hf_std { get; set; }
        public double? resid_std_mean { get; set; }
        public double? resid_std_std { get; set; }
        public List<FrameMetrics>? frames { get; set; }
    }
    public sealed class AudioMetrics
    {
        public double? audio_flatness_score { get; set; }
    }

    public class ForensicsState
    {
        public ForensicsResult? LastResult { get; set; }
    }
}
