Console.WriteLine("EAxWiki - Sparx EA Repository to Wiki Generator");

var config = new EAxWiki.Config();
config.Load(args);

if (string.IsNullOrEmpty(config.RepositoryPath))
{
    Console.WriteLine("Usage: EAxWiki --repo <path-to-eap-file> [--output <wiki-dir>]");
    return;
}

Console.WriteLine($"Repository: {config.RepositoryPath}");
Console.WriteLine($"Output:     {config.OutputPath}");
Console.WriteLine($"Package:    {config.PackageFilter ?? "(all)"}");
