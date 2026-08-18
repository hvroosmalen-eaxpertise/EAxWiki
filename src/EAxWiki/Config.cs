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
    public string? CertPath { get; set; }
    public string? CertPassword { get; set; }
    public string AiEndpoint { get; set; } = "";
    public string AiModel { get; set; } = "llama-3.2-3b";
    public string AiKey { get; set; } = "";
    public string Brand { get; set; } = "";
}
