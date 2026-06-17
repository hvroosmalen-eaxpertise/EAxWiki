using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.EA;
using EAxWiki.Export;

Console.WriteLine("EAxWiki - Sparx EA Repository to Wiki Generator");
Console.WriteLine();

var config = new EAxWiki.Config();
config.Load(args);

if (config.HelpRequested)
{
    ShowUsage();
    return;
}

var outputPath = Path.GetFullPath(config.OutputPath);
Console.WriteLine($"Repository: {config.RepositoryPath}");
Console.WriteLine($"Output:     {outputPath}");
if (!string.IsNullOrEmpty(config.PackageFilter))
    Console.WriteLine($"Package:    {config.PackageFilter}");
Console.WriteLine();

IEaReader reader = new EaReader();
IOutputWriter writer = new FileOutputWriter();
IWikiExporter exporter = new MarkdownExporter(writer);

try
{
    var repository = reader.Open(config.RepositoryPath);

    EaPackage? startPackage = null;
    if (!string.IsNullOrEmpty(config.PackageFilter))
    {
        startPackage = FindPackage(repository.RootPackages, config.PackageFilter);
        if (startPackage == null)
            Console.WriteLine($"Warning: Package '{config.PackageFilter}' not found. Exporting entire repository.");
    }

    await exporter.ExportAsync(repository, startPackage, outputPath, reader);
    Console.WriteLine($"Done. Wiki generated at: {outputPath}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    if (reader is IDisposable d) d.Dispose();
}

static EaPackage? FindPackage(List<EaPackage> packages, string name)
{
    foreach (var pkg in packages)
    {
        if (string.Equals(pkg.Name, name, StringComparison.OrdinalIgnoreCase))
            return pkg;
        var found = FindPackage(pkg.Children, name);
        if (found != null) return found;
    }
    return null;
}

static void ShowUsage()
{
    Console.WriteLine("Usage: EAxWiki [options]");
    Console.WriteLine();
    Console.WriteLine("Options:");
    Console.WriteLine("  --repo, -r <path>     Path to the EA repository file");
    Console.WriteLine("                        (default: M:\\EAxWiki\\model\\EurSuRA.qea)");
    Console.WriteLine("  --name, -n <name>     Display name for the repository");
    Console.WriteLine("  --output, -o <dir>    Output directory for the wiki");
    Console.WriteLine("                        (default: wiki)");
    Console.WriteLine("  --package, -p <name>  Only export a specific package (by name)");
    Console.WriteLine("  --help, -h            Show this help message");
}
