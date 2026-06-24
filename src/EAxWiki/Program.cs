using Microsoft.Extensions.Logging;
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

using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddSimpleConsole(options => options.TimestampFormat = "HH:mm:ss.fff ");
    builder.SetMinimumLevel(config.Verbose ? LogLevel.Debug : LogLevel.Information);
});

IEaReader reader = new EaReader();
IOutputWriter writer = new FileOutputWriter();
var logger = loggerFactory.CreateLogger<MarkdownExporter>();
IWikiExporter exporter = new MarkdownExporter(writer, logger);

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

    await exporter.ExportAsync(repository, startPackage, outputPath, reader, config.Force);
    Console.WriteLine($"Done. Wiki generated at: {outputPath}");

    if (config.JsonExport)
    {
        var jsonExporter = new JsonExporter(writer);
        await jsonExporter.ExportAsync(repository, outputPath);
        Console.WriteLine($"JSON export: {Path.Combine(outputPath, "model.json")}");
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    if (ex.InnerException != null)
        Console.WriteLine(ex.InnerException.ToString());
}
finally
{
    try
    {
        if (reader is IDisposable d) d.Dispose();
    }
    catch
    {
        // Ignore cleanup errors from EA COM
    }
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
    Console.WriteLine("  --verbose, -v         Enable verbose logging per-element timing");
    Console.WriteLine("  --force, -f           Force full regeneration (rebuild all files)");
    Console.WriteLine("  --json, -j            Also export model.json alongside markdown");
    Console.WriteLine("  --help, -h            Show this help message");
}
