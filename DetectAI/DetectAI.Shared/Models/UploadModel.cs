using Microsoft.AspNetCore.Components.Forms;

namespace DetectAI.Shared.Models
{
    public class UploadModel
    {
        public IReadOnlyList<IBrowserFile>? Files { get; set; } = new List<IBrowserFile>();
    }
}
