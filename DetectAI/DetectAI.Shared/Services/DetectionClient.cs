using System.Net.Http.Headers;
using System.Net.Http.Json;
using DetectAI.Shared.Models;

namespace DetectAI.Shared.Services;

public sealed class DetectionApiClient
{
    private readonly HttpClient _http;

    public DetectionApiClient(HttpClient http) => _http = http;

    public async Task<AnalyzeResult> AnalyzeAsync(Stream fileStream, string fileName, MediaType mediaType, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(streamContent, "file", fileName);
        content.Add(new StringContent(mediaType.ToString()), "mediaType");

        using var response = await _http.PostAsync("api/analyze", content, ct);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AnalyzeResult>(cancellationToken: ct);
        if (result is null) throw new InvalidOperationException("Empty response from analyze endpoint.");
        return result;
    }
}