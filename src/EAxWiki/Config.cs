namespace EAxWiki;

public class Config
{
    public string? RepositoryPath { get; set; }
    public string OutputPath { get; set; } = "wiki";
    public string? PackageFilter { get; set; }

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
                case "--output":
                case "-o":
                    OutputPath = args[++i];
                    break;
                case "--package":
                case "-p":
                    PackageFilter = args[++i];
                    break;
            }
        }
    }
}
