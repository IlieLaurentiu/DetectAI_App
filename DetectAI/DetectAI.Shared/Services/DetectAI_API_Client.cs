using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using DetectAI.Shared.Models;


namespace DetectAI.Shared.Services
{
    public class DetectAIApiClient
    {
        private readonly HttpClient _http;
        public DetectAIApiClient(HttpClient http) => _http = http;


        public async Task<bool> PingAsync()
        {
            try
            {
                var pong = await _http.GetStringAsync("/ping");
                return pong.Contains("pong");
            }
            catch { return false; }
        }


        public async Task<ForensicsResult?> AnalyzeAsync(IBrowserFile file)
        {
            using var content = new MultipartFormDataContent();
            await using var stream = file.OpenReadStream(200 * 1024 * 1024);
            using var fileContent = new StreamContent(stream);
            content.Add(fileContent, "file", file.Name);


            var res = await _http.PostAsync("/forensics/analyze?verbose=true", content);
            res.EnsureSuccessStatusCode();


            return await res.Content.ReadFromJsonAsync<ForensicsResult>();
        }
    }
}