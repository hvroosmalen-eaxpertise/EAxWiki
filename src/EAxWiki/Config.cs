namespace EAxWiki;

public class Config
{
    public string RepositoryPath { get; set; } = "M:\\EAxWiki\\model\\EurSuRA.qea";
    public string? RepositoryName { get; set; }
    public string OutputPath { get; set; } = "wiki";
    public string? PackageFilter { get; set; }
    public bool HelpRequested { get; set; }
    public bool Verbose { get; set; }
    public bool Force { get; set; }
    public bool JsonExport { get; set; }

    public void Load(string[] args)
    {
        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--repo":
                case "-r":
                    RepositoryPath = args[++i];
                    break;
                case "--name":
                case "-n":
                    RepositoryName = args[++i];
                    break;
                case "--output":
                case "-o":
                    OutputPath = args[++i];
                    break;
                case "--package":
                case "-p":
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
                case "--help":
                case "/?":
                case "-h":
                    HelpRequested = true;
                    break;
            }
        }
    }
}
