namespace EAxWiki;

public class Config
{
    public string RepositoryPath { get; set; } = string.Empty;
    public string? RepositoryName { get; set; }
    public string OutputPath { get; set; } = "wiki";
    public string? PackageFilter { get; set; }
    public bool HelpRequested { get; set; }
    public bool Verbose { get; set; }
    public bool Force { get; set; }
    public bool JsonExport { get; set; }
    public bool WriteBack { get; set; }
    public bool ApiMode { get; set; }
    public int ApiPort { get; set; } = 0;
    public int WikiPort { get; set; } = 0;
    public string? CertPath { get; set; }
    public string? CertPassword { get; set; }
    public string AiEndpoint { get; set; } = "";
    public string AiModel { get; set; } = "llama-3.2-3b";
    public string AiKey { get; set; } = "";

    public void Load(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--repo":
                case "-r":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    RepositoryPath = args[++i];
                    break;
                case "--name":
                case "-n":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    RepositoryName = args[++i];
                    break;
                case "--output":
                case "-o":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    OutputPath = args[++i];
                    break;
                case "--package":
                case "-p":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    PackageFilter = args[++i];
                    break;
                case "--verbose":
                case "-v":
                    Verbose = true;
                    break;
                case "--force":
                case "-f":
                    Force = true;
                    break;
                case "--json":
                case "-j":
                    JsonExport = true;
                    break;
                case "--writeback":
                case "-w":
                    WriteBack = true;
                    break;
                case "--api":
                    ApiMode = true;
                    if (ApiPort == 0) ApiPort = 8001;
                    break;
                case "--api-port":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    ApiPort = int.Parse(args[++i]);
                    break;
                case "--wiki-port":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    WikiPort = int.Parse(args[++i]);
                    break;
                case "--cert":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    CertPath = args[++i];
                    break;
                case "--cert-password":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    CertPassword = args[++i];
                    break;
                case "--ai-endpoint":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    AiEndpoint = args[++i];
                    break;
                case "--ai-model":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    AiModel = args[++i];
                    break;
                case "--ai-key":
                    if (i + 1 >= args.Length)
                        throw new ArgumentException($"Option {args[i]} requires a value");
                    AiKey = args[++i];
                    break;
                case "--help":
                case "/?":
                case "-h":
                    HelpRequested = true;
                    break;
            }
        }
    }
}
