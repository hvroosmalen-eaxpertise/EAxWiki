namespace EAxWiki;

public class Config
{
    public string RepositoryPath { get; set; } = string.Empty;
    public string? RepositoryName { get; set; }
    public string OutputPath { get; set; } = "wiki";
    public string? PackageFilter { get; set; }
    public bool Verbose { get; set; }
    public bool Force { get; set; }
    public bool JsonExport { get; set; }
    public bool WriteBack { get; set; }
    public bool ApiMode { get; set; }
    public int ApiPort { get; set; } = 0;
    public int WikiPort { get; set; } = 0;
    public int ApiRateLimitPerMinute { get; set; } = 60;
    /// <summary>
    /// Optional path where the API writes its readiness signal (created on ApplicationStarted,
    /// deleted on shutdown). When null, no ready file is written. The monitor sets this to a path
    /// under its state dir (outside the wiki output) so mkdocs's file walk never sees the file
    /// flicker in/out during API restarts.
    /// </summary>
    public string? ReadyFile { get; set; }
    public string? CertPath { get; set; }
    public string? CertPassword { get; set; }
    public string AiEndpoint { get; set; } = "";
    public string AiModel { get; set; } = "llama-3.2-3b";
    public string AiKey { get; set; } = "";
    public string Brand { get; set; } = "";
}
