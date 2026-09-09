using System.Text.Json;
using EAxWiki.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Export.Exporters;

internal abstract class ExporterBase
{
    protected readonly IOutputWriter Writer;
    protected readonly ILogger Logger;

    protected ExporterBase(IOutputWriter writer, ILogger logger)
    {
        Writer = writer;
        Logger = logger;
    }

    protected Task WriteJsonFileAsync(string path, object value, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = false });
        return Writer.WriteFileAsync(path, json, ct);
    }
}
