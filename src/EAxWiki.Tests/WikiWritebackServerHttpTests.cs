using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using EAxWiki;
using EAxWiki.Core.Interfaces;
using EAxWiki.Tests.TestDoubles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace EAxWiki.Tests;

/// <summary>
/// HTTP-level tests for WikiWritebackServer.Configure. Uses AspNetCore.TestHost to run the real
/// middleware pipeline (origin check, token auth, rate limiter) and endpoints against an in-memory
/// HttpClient — no ports, no Kestrel, no COM. The reader is a <see cref="FakeEaReader"/> so writes
/// are recorded but never touch EA.
/// </summary>
public class WikiWritebackServerHttpTests : IAsyncLifetime
{
    private string _outputDir = string.Empty;
    private string _token = string.Empty;
    private WebApplication _app = null!;
    private HttpClient _client = null!;
    private FakeEaReader _reader = null!;

    private const int WikiPort = 8000;
    private const string Origin = "http://localhost:8000";

    public async Task InitializeAsync()
    {
        _outputDir = Path.Combine(Path.GetTempPath(), "eaxwiki_http_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_outputDir);
        _token = EAxWiki.Export.Helpers.ApiTokenStore.GetOrCreate(_outputDir);
        _reader = new FakeEaReader();

        // A page file that the mutation endpoints can find on disk. Content mirrors what the real
        // exporter writes for an element: frontmatter with ea_id + status + hashes, then the notes
        // widget with the ea-notes-start/end markers UpdateNotes patches into.
        var pagePath = Path.Combine(_outputDir, "test-element.md");
        File.WriteAllText(pagePath, string.Join("\n",
        [
            "---",
            "ea_id: 42",
            "status: Proposed",
            "ea_hash: " + EAxWiki.Export.Renderers.HtmlHelpers.ComputeStatusHash("Proposed"),
            "notes_hash: " + EAxWiki.Export.Renderers.HtmlHelpers.ComputeNotesHash(""),
            "---",
            "",
            "# Test",
            "",
            "**Status:** <span class=\"status-badge status-proposed\">Proposed</span>",
            "**Modified:** 2026-01-01",
            "",
            "<div id=\"ea-status-editor\" data-ea-id=\"42\" data-status=\"Proposed\">",
            "<!--ea-notes-start-->",
            "",
            "<!--ea-notes-end-->",
            "</div>",
            "",
        ]));

        var builder = WebApplication.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.WebHost.UseTestServer();
        _app = builder.Build();

        var config = new Config { WikiPort = WikiPort, ApiPort = 8001 };
        WikiWritebackServer.Configure(_app, _reader, config, _outputDir, NullLogger.Instance);

        await _app.StartAsync();
        _client = _app.GetTestClient();
    }

    public async Task DisposeAsync()
    {
        _client?.Dispose();
        if (_app != null) await _app.DisposeAsync();
        if (Directory.Exists(_outputDir))
            try { Directory.Delete(_outputDir, recursive: true); } catch { }
    }

    private HttpRequestMessage Post(string path, object body, string? token = null, string? origin = Origin)
    {
        var req = new HttpRequestMessage(HttpMethod.Post, path) { Content = JsonContent.Create(body) };
        if (origin != null) req.Headers.Add("Origin", origin);
        req.Headers.Add("X-EAxWiki-Token", token ?? _token);
        return req;
    }

    // ─── auth ────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task ApiCall_WithoutToken_Returns401()
    {
        var req = new HttpRequestMessage(HttpMethod.Post, "/api/status")
        {
            Content = JsonContent.Create(new { elementId = 42, newStatus = "Approved", filePath = "test-element.md" })
        };
        req.Headers.Add("Origin", Origin);

        var response = await _client.SendAsync(req);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ApiCall_WithWrongToken_Returns401()
    {
        var response = await _client.SendAsync(
            Post("/api/status", new { elementId = 42, newStatus = "Approved", filePath = "test-element.md" }, token: "not-the-real-token"));
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task ApiCall_ResponseBody_DoesNotContainRealToken()
    {
        var response = await _client.SendAsync(
            Post("/api/status", new { elementId = 42, newStatus = "Approved", filePath = "test-element.md" }, token: "not-the-real-token"));
        var body = await response.Content.ReadAsStringAsync();
        // Fix from earlier audit: 401 must not echo either provided or expected token — otherwise
        // the FixedTimeEquals compare on the request path is pointless.
        Assert.DoesNotContain(_token, body);
        Assert.DoesNotContain("not-the-real-token", body);
    }

    // ─── mutation happy path ─────────────────────────────────────────────────────────────────

    [Fact]
    public async Task PostStatus_WithValidToken_RecordsWriteAndReturns200()
    {
        var response = await _client.SendAsync(
            Post("/api/status", new { elementId = 42, newStatus = "Approved", filePath = "test-element.md" }));
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Single(_reader.StatusUpdates);
        Assert.Equal((42, "Approved"), _reader.StatusUpdates[0]);
    }

    [Fact]
    public async Task PostStatus_UnknownStatus_Returns400()
    {
        var response = await _client.SendAsync(
            Post("/api/status", new { elementId = 42, newStatus = "TotallyMadeUp", filePath = "test-element.md" }));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Empty(_reader.StatusUpdates);
    }

    [Fact]
    public async Task PostStatus_MissingFile_Returns404()
    {
        var response = await _client.SendAsync(
            Post("/api/status", new { elementId = 42, newStatus = "Approved", filePath = "does-not-exist.md" }));
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task PostStatus_PathTraversal_Returns400()
    {
        var response = await _client.SendAsync(
            Post("/api/status", new { elementId = 42, newStatus = "Approved", filePath = "../secret.md" }));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    // ─── /readyz ─────────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Readyz_WhenReaderHealthy_Returns200()
    {
        _reader.IsHealthySignal = true;
        var response = await _client.GetAsync("/readyz");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"ea\":true", json);
    }

    [Fact]
    public async Task Readyz_WhenReaderUnhealthy_Returns503()
    {
        _reader.IsHealthySignal = false;
        var response = await _client.GetAsync("/readyz");
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
    }

    // ─── CORS ────────────────────────────────────────────────────────────────────────────────

    [Fact]
    public async Task CorsHeader_OnMatchingOrigin_IsEchoed()
    {
        var req = new HttpRequestMessage(HttpMethod.Options, "/api/status");
        req.Headers.Add("Origin", Origin);
        var response = await _client.SendAsync(req);
        Assert.Equal(Origin, response.Headers.GetValues("Access-Control-Allow-Origin").Single());
    }

    [Fact]
    public async Task CorsHeader_OnMismatchedPort_IsNotSet()
    {
        // Middleware only sets Access-Control-Allow-Origin when the origin's port matches --wiki-port,
        // scoping trust to the one wiki instance this server was started for.
        var req = new HttpRequestMessage(HttpMethod.Options, "/api/status");
        req.Headers.Add("Origin", "http://localhost:9999");
        var response = await _client.SendAsync(req);
        Assert.False(response.Headers.Contains("Access-Control-Allow-Origin"));
    }
}
