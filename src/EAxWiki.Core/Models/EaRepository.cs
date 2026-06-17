namespace EAxWiki.Core.Models;

public class EaRepository
{
    public string Name { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public List<EaPackage> RootPackages { get; set; } = new();
}
