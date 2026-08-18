using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Logging;
using EAxWiki;
using EAxWiki.Core.Configuration;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using EAxWiki.EA;
using EAxWiki.Export;
using EAxWiki.Export.Exporters;

Console.WriteLine("EAxWiki - Sparx EA Repository to Wiki Generator");
Console.WriteLine();

var root = CommandLine.BuildCommand();
var parseResult = root.Parse(args);
if (parseResult.Errors.Count > 0)
{
    await parseResult.InvokeAsync(new InvocationConfiguration(), CancellationToken.None);
    return 1;
}

if (parseResult.Action is HelpAction)
{
    await parseResult.InvokeAsync(new InvocationConfiguration(), CancellationToken.None);
    return 0;
}

var config = CommandLine.ToConfig(parseResult);

const string ConfigFileName = ".eaxwiki";

// Find .eaxwiki in current directory or parent directories, but stop at repo root (.git)
string LocalConfig = "";
var currentDir = Directory.GetCurrentDirectory();
var searchDir = currentDir;
var repoRoot = currentDir; // Track the repo root
for (int i = 0; i < 10; i++)
{
    var candidate = Path.Combine(searchDir, ConfigFileName);
    if (File.Exists(candidate))
    {
        LocalConfig = candidate;
        break;
    }

    // Stop searching if we find .git (repo root)
    if (Directory.Exists(Path.Combine(searchDir, ".git")))
    {
        repoRoot = searchDir;
        break;
    }

    var parent = Directory.GetParent(searchDir);
    if (parent == null) break;
    searchDir = parent.FullName;
}

// If not found, default to repo root
if (string.IsNullOrEmpty(LocalConfig))
{
    LocalConfig = Path.Combine(repoRoot, ConfigFileName);
}

// Always try to load .eaxwiki for fallback values (ports, AI endpoint, etc.)
LocalConfigStore.Config? savedConfig = null;
bool wasLegacyPlaintext = false;
if (!string.IsNullOrWhiteSpace(LocalConfig) && File.Exists(LocalConfig))
{
    savedConfig = LocalConfigStore.Load(LocalConfig, out wasLegacyPlaintext);
}

if (string.IsNullOrWhiteSpace(config.RepositoryPath))
{
    if (savedConfig != null && !string.IsNullOrWhiteSpace(savedConfig.RepoPath))
    {
        config.RepositoryPath = savedConfig.RepoPath;

        Console.WriteLine($"Using saved repository: {EaRepository.Redact(config.RepositoryPath)}");
        if (wasLegacyPlaintext)
        {
            LocalConfigStore.Save(LocalConfig, savedConfig);
            Console.WriteLine($"(Encrypted {LocalConfig} at rest — it was stored in plaintext.)");
        }
        Console.WriteLine($"(Pass --repo to override, or delete {Path.GetFileName(LocalConfig)} to re-enter interactively.)");
        Console.WriteLine();
    }
    else
    {
        config.RepositoryPath = BuildConnectionStringInteractively();
        if (!string.IsNullOrWhiteSpace(config.RepositoryPath))
        {
            // Validate the repository path before saving config
            bool isConnectionString = config.RepositoryPath.Contains('=');
            if (!isConnectionString && !File.Exists(config.RepositoryPath))
            {
                Console.Error.WriteLine($"Error: repository file not found: {config.RepositoryPath}");
                Console.Error.WriteLine("Please use an absolute path like: E:\\Users\\Han\\Repos\\EAxWiki\\model\\EurSuRA.qea");
                return 1;
            }

            Console.WriteLine();
            Console.Write("Wiki serve port [8000]: ");
            var wikiPortStr = (Console.ReadLine() ?? "").Trim();
            var wikiPort = string.IsNullOrEmpty(wikiPortStr) ? 8000 : int.Parse(wikiPortStr);

            Console.Write($"API port [{wikiPort + 1}]: ");
            var apiPortStr = (Console.ReadLine() ?? "").Trim();
            var apiPort = string.IsNullOrEmpty(apiPortStr) ? wikiPort + 1 : int.Parse(apiPortStr);

            Console.Write("Configure Slack webhook for monitoring alerts? [y/N]: ");
            var wantWebhook = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            var webhookUrl = "";
            if (wantWebhook == "y" || wantWebhook == "yes")
            {
                Console.Write("Slack webhook URL (https://hooks.slack.com/services/...): ");
                webhookUrl = (Console.ReadLine() ?? "").Trim();
            }

            Console.Write("Configure Teams webhook for monitoring alerts? [y/N]: ");
            var wantTeamsWebhook = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            var teamsWebhookUrl = "";
            if (wantTeamsWebhook == "y" || wantTeamsWebhook == "yes")
            {
                Console.Write("Teams webhook URL (from a Workflows \"Send webhook alert to a channel\" flow, or a classic Connector): ");
                teamsWebhookUrl = (Console.ReadLine() ?? "").Trim();
            }

            Console.Write("Configure Telegram monitoring alerts? [y/N]: ");
            var wantTelegram = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            var telegramBotToken = "";
            var telegramChatId = "";
            if (wantTelegram == "y" || wantTelegram == "yes")
            {
                Console.Write("Telegram bot token (from @BotFather): ");
                telegramBotToken = (Console.ReadLine() ?? "").Trim();
                Console.Write("Telegram chat ID (numeric, the destination chat): ");
                telegramChatId = (Console.ReadLine() ?? "").Trim();
            }

            var newConfig = new LocalConfigStore.Config
            {
                RepoPath = config.RepositoryPath,
                WebhookUrl = webhookUrl,
                TeamsWebhookUrl = teamsWebhookUrl,
                TelegramBotToken = telegramBotToken,
                TelegramChatId = telegramChatId,
                WikiPort = wikiPort,
                ApiPort = apiPort
            };
            LocalConfigStore.Save(LocalConfig, newConfig);
            Console.WriteLine($"Saved to {LocalConfig} (encrypted) — future runs will use this automatically.");
            Console.WriteLine();

            // Reload savedConfig with freshly-saved values so fallbacks below apply correctly
            if (!string.IsNullOrWhiteSpace(LocalConfig) && File.Exists(LocalConfig))
            {
                savedConfig = LocalConfigStore.Load(LocalConfig, out _);
            }
        }
    }
}

// Apply .eaxwiki fallbacks for ApiPort, WikiPort, and AI settings
// (always, even when --repo was provided — the monitor passes --repo but
//  relies on .eaxwiki for ApiPort and AI config)
if (savedConfig != null)
{
    if (config.WikiPort == 0 && savedConfig.WikiPort.HasValue)
        config.WikiPort = savedConfig.WikiPort.Value;
    if (config.ApiPort == 0 && savedConfig.ApiPort.HasValue)
        config.ApiPort = savedConfig.ApiPort.Value;
    if (string.IsNullOrEmpty(config.AiEndpoint) && !string.IsNullOrEmpty(savedConfig.AiEndpoint))
        config.AiEndpoint = savedConfig.AiEndpoint;
    if (string.IsNullOrEmpty(config.AiModel) && !string.IsNullOrEmpty(savedConfig.AiModel))
        config.AiModel = savedConfig.AiModel;
    if (string.IsNullOrEmpty(config.AiKey) && !string.IsNullOrEmpty(savedConfig.AiKey))
        config.AiKey = savedConfig.AiKey;
    if (string.IsNullOrEmpty(config.Brand) && !string.IsNullOrEmpty(savedConfig.Brand))
        config.Brand = savedConfig.Brand;
}

if (string.IsNullOrWhiteSpace(config.RepositoryPath))
{
    Console.Error.WriteLine("Error: no repository specified.");
    return 1;
}

var outputPath = Path.GetFullPath(config.OutputPath);

// Verify the output parent directory is reachable before doing anything destructive.
var outputParent = Path.GetDirectoryName(outputPath) ?? outputPath;
if (!string.IsNullOrEmpty(outputParent) && !Directory.Exists(outputParent))
{
    Console.Error.WriteLine($"Error: output parent directory does not exist: {outputParent}");
    return 1;
}

Console.WriteLine($"Repository: {EaRepository.Redact(config.RepositoryPath)}");
Console.WriteLine($"Output:     {outputPath}");
if (!string.IsNullOrEmpty(config.PackageFilter))
    Console.WriteLine($"Package:    {config.PackageFilter}");
if (config.WriteBack)
    Console.WriteLine("Mode:       write-back enabled (wiki → EA)");
if (config.ApiMode)
    Console.WriteLine($"Mode:       wiki write-back server on port {config.ApiPort}");
Console.WriteLine();

using var loggerFactory = LoggerFactory.Create(builder =>
{
    if (Console.Error is StreamWriter sw) sw.AutoFlush = true;
    builder.AddSimpleConsole(options => options.TimestampFormat = "HH:mm:ss.fff ");
    builder.SetMinimumLevel(config.Verbose ? LogLevel.Debug : LogLevel.Information);
});

IEaReader reader = new EaReader(loggerFactory.CreateLogger<EaReader>());
var writer = new FileOutputWriter();
var logger = loggerFactory.CreateLogger<MarkdownExporter>();
IWikiExporter exporter = new MarkdownExporter(writer, logger);

if (config.ApiMode)
{
    await WikiWritebackServer.RunAsync(config, outputPath, loggerFactory);
    return 0;
}

// Expose API port to MarkdownExporter so the status-editor widget URL is embedded correctly.
Environment.SetEnvironmentVariable("EAXWIKI_API_PORT", config.ApiPort.ToString());
if (!string.IsNullOrEmpty(config.AiEndpoint))
    Environment.SetEnvironmentVariable("EAXWIKI_AI_ENDPOINT", config.AiEndpoint);
Environment.SetEnvironmentVariable("EAXWIKI_BRAND", config.Brand);

try
{
    var repository = reader.Open(config.RepositoryPath);

    if (config.WriteBack && Directory.Exists(outputPath))
    {
        Console.WriteLine("Running write-back scan...");
        var scanner = new WriteBackScanner(reader, loggerFactory.CreateLogger<WriteBackScanner>());
        var scanResult = scanner.Scan(outputPath);
        if (scanResult.StatusChanges.Count == 0)
            Console.WriteLine("Write-back: no status changes detected.");
        else
            Console.WriteLine($"Write-back: applied {scanResult.StatusChanges.Count} status change(s) to EA.");
        if (scanResult.NotesChanges.Count == 0)
            Console.WriteLine("Write-back: no notes changes detected.");
        else
            Console.WriteLine($"Write-back: applied {scanResult.NotesChanges.Count} notes change(s) to EA.");
        Console.WriteLine();
    }

    EaPackage? startPackage = null;
    if (!string.IsNullOrEmpty(config.PackageFilter))
    {
        startPackage = FindPackage(repository.RootPackages, config.PackageFilter);
        if (startPackage == null)
            Console.WriteLine($"Warning: Package '{config.PackageFilter}' not found. Exporting entire repository.");
    }

    var result = await exporter.ExportAsync(repository, startPackage, outputPath, reader, config.Force);
    Console.WriteLine($"Done. Wiki generated at: {outputPath}");
    if (result.FailedElements > 0)
        Console.WriteLine($"  {result.SucceededElements} succeeded, {result.FailedElements} failed, {result.Elapsed.TotalSeconds:F1}s");
    else
        Console.WriteLine($"  {result.SucceededElements} elements exported in {result.Elapsed.TotalSeconds:F1}s");

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
    return 1;
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

return 0;

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

    Console.Write("Port (leave blank for default): ");
    var port = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Database name: ");
    var database = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Username: ");
    var user = Console.ReadLine()?.Trim() ?? string.Empty;

    Console.Write("Password: ");
    var password = ReadPassword();

    // SQL Server appends port with a comma: "SERVER,1433"
    var sqlServerHost = string.IsNullOrEmpty(port) ? server : $"{server},{port}";
    // MySQL / PostgreSQL use a separate Port= key
    var portSegment = string.IsNullOrEmpty(port) ? "" : $"Port={port};";

    return choice switch
    {
        "2" => $"DBType=1;Connect=Provider=SQLOLEDB.1;Data Source={sqlServerHost};Initial Catalog={database};User Id={user};Password={password};",
        "3" => $"DBType=3;Connect=Server={server};{portSegment}Database={database};Uid={user};Pwd={password};",
        "4" => $"DBType=2;Connect=Data Source={server};User Id={user};Password={password};",
        "5" => $"DBType=7;Connect=Server={server};{portSegment}Database={database};User Id={user};Password={password};",
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
