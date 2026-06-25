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

if (string.IsNullOrWhiteSpace(config.RepositoryPath))
    config.RepositoryPath = BuildConnectionStringInteractively();

if (string.IsNullOrWhiteSpace(config.RepositoryPath))
{
    Console.Error.WriteLine("Error: no repository specified.");
    return;
}

// Only validate file existence for plain file paths, not DB connection strings.
bool isConnectionString = config.RepositoryPath.Contains('=');
if (!isConnectionString && !File.Exists(config.RepositoryPath))
{
    Console.Error.WriteLine($"Error: repository file not found: {config.RepositoryPath}");
    return;
}

var outputPath = Path.GetFullPath(config.OutputPath);

// Verify the output parent directory is reachable before doing anything destructive.
var outputParent = Path.GetDirectoryName(outputPath) ?? outputPath;
if (!string.IsNullOrEmpty(outputParent) && !Directory.Exists(outputParent))
{
    Console.Error.WriteLine($"Error: output parent directory does not exist: {outputParent}");
    return;
}

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

IEaReader reader = new EaReader(loggerFactory.CreateLogger<EaReader>());
using var writer = new FileOutputWriter();
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
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Warning: EA cleanup failed: {ex.Message}");
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
    Console.WriteLine("  --repo, -r <value>    Path to a .qea file, or a DB connection string.");
    Console.WriteLine("                        Omit to enter interactive connection builder.");
    Console.WriteLine("  --name, -n <name>     Display name for the repository");
    Console.WriteLine("  --output, -o <dir>    Output directory for the wiki (default: wiki)");
    Console.WriteLine("  --package, -p <name>  Only export a specific package (by name)");
    Console.WriteLine("  --verbose, -v         Enable verbose logging per-element timing");
    Console.WriteLine("  --force, -f           Force full regeneration (rebuild all files)");
    Console.WriteLine("  --json, -j            Also export model.json alongside markdown");
    Console.WriteLine("  --help, -h            Show this help message");
    Console.WriteLine();
    Console.WriteLine("Connection string examples:");
    Console.WriteLine("  SQL Server:  DBType=1;Connect=Provider=SQLOLEDB.1;Data Source=SERVER;Initial Catalog=EA;Integrated Security=SSPI;");
    Console.WriteLine("  MySQL:       DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;");
    Console.WriteLine("  MariaDB:     DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;");
    Console.WriteLine("  Oracle:      DBType=2;Connect=Data Source=TNSNAME;User Id=user;Password=pass;");
    Console.WriteLine("  PostgreSQL:  DBType=7;Connect=Server=localhost;Database=EA;User Id=user;Password=pass;");
}

static string BuildConnectionStringInteractively()
{
    Console.WriteLine("No --repo specified. Enter repository details interactively.");
    Console.WriteLine();
    Console.WriteLine("Repository type:");
    Console.WriteLine("  1) File (.qea)");
    Console.WriteLine("  2) SQL Server");
    Console.WriteLine("  3) MySQL / MariaDB");
    Console.WriteLine("  4) Oracle");
    Console.WriteLine("  5) PostgreSQL");
    Console.Write("Choice [1]: ");
    var choice = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(choice)) choice = "1";

    if (choice == "1")
    {
        Console.Write("Path to .qea file: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    Console.Write("Server / host: ");
    var server = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Database name: ");
    var database = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Username: ");
    var user = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Password: ");
    var password = ReadPassword();

    return choice switch
    {
        "2" => $"DBType=1;Connect=Provider=SQLOLEDB.1;Data Source={server};Initial Catalog={database};User Id={user};Password={password};",
        "3" => $"DBType=3;Connect=Server={server};Database={database};Uid={user};Pwd={password};",
        "4" => $"DBType=2;Connect=Data Source={server};User Id={user};Password={password};",
        "5" => $"DBType=7;Connect=Server={server};Database={database};User Id={user};Password={password};",
        _   => string.Empty
    };
}

static string ReadPassword()
{
    var password = new System.Text.StringBuilder();
    while (true)
    {
        var key = Console.ReadKey(intercept: true);
        if (key.Key == ConsoleKey.Enter) { Console.WriteLine(); break; }
        if (key.Key == ConsoleKey.Backspace && password.Length > 0) { password.Remove(password.Length - 1, 1); continue; }
        if (key.KeyChar != '\0') password.Append(key.KeyChar);
    }
    return password.ToString();
}
