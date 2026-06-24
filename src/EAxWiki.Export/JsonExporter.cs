using System.Text.Json;
using System.Text.Json.Serialization;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;

namespace EAxWiki.Export;

public class JsonExporter
{
    private readonly IOutputWriter _writer;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        ReferenceHandler = ReferenceHandler.IgnoreCycles,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public JsonExporter(IOutputWriter writer)
    {
        _writer = writer;
    }

    public async Task ExportAsync(EaRepository repository, string outputPath)
    {
        var json = JsonSerializer.Serialize(repository, JsonOptions);
        var filePath = Path.Combine(outputPath, "model.json");
        await _writer.WriteFileAsync(filePath, json);
    }
}
