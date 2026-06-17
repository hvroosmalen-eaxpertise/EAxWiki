namespace EAxWiki.Core.Configuration;

public class EaWikiConfig
{
    public string RepositoryPath { get; set; } = string.Empty;
    public string RepositoryName { get; set; } = string.Empty;
    public string OutputPath { get; set; } = "wiki";
    public string? StartPackage { get; set; }
}
