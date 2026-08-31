using System.CommandLine;

namespace EAxWiki;

public static class CommandLine
{
    private static readonly Argument<string?> RepoArg = new("repo")
    {
        Description = "Path to a .qea file, or a DB connection string. Omit to enter interactive connection builder.",
        // Default arity for string? is ExactlyOne (GetValue throws "Required argument missing" when absent);
        // ZeroOrOne lets no-arg invocations parse cleanly and keeps GetValue -> null.
        Arity = ArgumentArity.ZeroOrOne,
    };

    private static readonly Option<string?> Repo = new("--repo", "-r")
    {
        Description = "Path to a .qea file, or a DB connection string (takes precedence over a bare positional repo).",
    };
    private static readonly Option<string?> Name = new("--name", "-n")
    {
        Description = "Display name for the repository.",
    };
    private static readonly Option<string?> Output = new("--output", "-o")
    {
        Description = "Output directory for the wiki (default: wiki).",
    };
    private static readonly Option<string?> Package = new("--package", "-p")
    {
        Description = "Only export a specific package (by name).",
    };
    private static readonly Option<bool> Verbose = new("--verbose", "-v")
    {
        Description = "Enable verbose logging per-element timing.",
    };
    private static readonly Option<bool> Force = new("--force", "-f")
    {
        Description = "Force full regeneration (rebuild all files).",
    };
    private static readonly Option<bool> Json = new("--json", "-j")
    {
        Description = "Also export model.json alongside markdown.",
    };
    private static readonly Option<bool> WriteBack = new("--writeback", "-w")
    {
        Description = "Scan wiki for status changes and write them back to EA via COM.",
    };
    private static readonly Option<bool> Api = new("--api")
    {
        Description = "Start wiki write-back server for in-wiki status editing.",
    };
    private static readonly Option<int?> ApiPort = new("--api-port")
    {
        Description = "Port for the wiki write-back server (default: 8001 when --api is given).",
    };
    private static readonly Option<int?> WikiPort = new("--wiki-port")
    {
        Description = "Port the paired 'mkdocs serve' uses (default: 8000); --api only accepts requests whose Origin matches this port.",
    };
    private static readonly Option<string?> ReadyFile = new("--ready-file")
    {
        Description = "Path where the API writes its readiness signal (created on start, deleted on shutdown). The monitor sets this; direct-run users can omit it.",
    };
    private static readonly Option<string?> Cert = new("--cert")
    {
        Description = "Path to PFX certificate for HTTPS.",
    };
    private static readonly Option<string?> CertPassword = new("--cert-password")
    {
        Description = "PFX certificate password.",
    };
    private static readonly Option<string?> AiEndpoint = new("--ai-endpoint")
    {
        Description = "OpenAI-compatible API base URL (empty = AI suggestions disabled).",
    };
    private static readonly Option<string?> AiModel = new("--ai-model")
    {
        Description = "Model name sent to AI endpoint (default: llama-3.2-3b).",
    };
    private static readonly Option<string?> AiKey = new("--ai-key")
    {
        Description = "API key for AI endpoint (optional for local LLMs).",
    };
    private static readonly Option<string?> Brand = new("--brand")
    {
        Description = "Brand theme to emit (eursura); default: none.",
    };

    private const string Description = """
        EAxWiki - Sparx EA Repository to Wiki Generator

        Connection string examples:
          SQL Server:  DBType=1;Connect=Provider=SQLOLEDB.1;Data Source=SERVER;Initial Catalog=EA;Integrated Security=SSPI;
          MySQL:       DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;
          MariaDB:     DBType=3;Connect=Server=localhost;Database=EA;Uid=user;Pwd=pass;
          Oracle:      DBType=2;Connect=Data Source=TNSNAME;User Id=user;Password=pass;
          PostgreSQL:  DBType=7;Connect=Server=localhost;Database=EA;User Id=user;Password=pass;
        """;

    public static RootCommand BuildCommand()
    {
        AddPortValidation(ApiPort);
        AddPortValidation(WikiPort);
        AddRepoArgValidation(RepoArg);

        var root = new RootCommand(Description)
        {
            RepoArg, Repo, Name, Output, Package, Verbose, Force, Json, WriteBack,
            Api, ApiPort, WikiPort, ReadyFile, Cert, CertPassword, AiEndpoint, AiModel, AiKey, Brand,
        };
        // A bare positional repo is the only legitimate non-option token; anything else is a typo.
        root.TreatUnmatchedTokensAsErrors = true;
        return root;
    }

    public static Config ToConfig(ParseResult r)
    {
        var api = r.GetValue(Api);
        var apiPort = r.GetValue(ApiPort);
        return new Config
        {
            RepositoryPath = r.GetValue(Repo) ?? r.GetValue(RepoArg) ?? "",
            RepositoryName = r.GetValue(Name),
            OutputPath = r.GetValue(Output) ?? "wiki",
            PackageFilter = r.GetValue(Package),
            Verbose = r.GetValue(Verbose),
            Force = r.GetValue(Force),
            JsonExport = r.GetValue(Json),
            WriteBack = r.GetValue(WriteBack),
            ApiMode = api,
            ApiPort = apiPort ?? (api ? 8001 : 0),
            WikiPort = r.GetValue(WikiPort) ?? 0,
            ReadyFile = r.GetValue(ReadyFile),
            CertPath = r.GetValue(Cert),
            CertPassword = r.GetValue(CertPassword),
            AiEndpoint = r.GetValue(AiEndpoint) ?? "",
            AiModel = r.GetValue(AiModel) ?? "llama-3.2-3b",
            AiKey = r.GetValue(AiKey) ?? "",
            Brand = r.GetValue(Brand) ?? "",
        };
    }

    private static void AddPortValidation(Option<int?> opt)
    {
        opt.Validators.Add(result =>
        {
            // Read the raw token, not GetValueOrDefault: the latter THROWS on non-numeric input,
            // while SCL already records a clean "Cannot parse argument" error for that case.
            var token = result.Tokens.Count > 0 ? result.Tokens[^1].Value : null;
            if (token is not null && int.TryParse(token, out var value) && value is < 1 or > 65535)
                result.AddError($"Option {opt.Name} requires an integer port in 1-65535 (got '{value}').");
        });
    }

    private static void AddRepoArgValidation(Argument<string?> arg)
    {
        // Without this, an unknown option-like token is swallowed by the bare argument as its value
        // (verified: '--froce' parsed with zero errors and repo='--froce'), defeating decision #2.
        arg.Validators.Add(result =>
        {
            var token = result.Tokens.Count > 0 ? result.Tokens[^1].Value : null;
            if (token is not null && token.StartsWith("-", StringComparison.Ordinal))
                result.AddError($"Unknown option '{token}'.");
        });
    }
}
