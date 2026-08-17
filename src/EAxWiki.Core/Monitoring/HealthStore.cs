using System.Text.Json;

namespace EAxWiki.Core.Monitoring;

/// <summary>
/// Load/save <see cref="HealthState"/> as JSON. Serialization uses camelCase property names
/// (matching the PS monitor's ConvertTo-Json output). Load backfills: fields missing from an
/// older on-disk file simply keep their CLR defaults (the equivalent of Add-Member -Force in
/// the PS monitor), and a corrupt file falls back to a fresh default state.
/// </summary>
public class HealthStore
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public HealthState Load(string path)
    {
        if (!File.Exists(path)) return new HealthState();
        try
        {
            var json = File.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(json)) return new HealthState();
            return JsonSerializer.Deserialize<HealthState>(json, Options) ?? new HealthState();
        }
        catch (JsonException)
        {
            return new HealthState();
        }
    }

    public void Save(string path, HealthState state)
    {
        var json = JsonSerializer.Serialize(state, Options);
        File.WriteAllText(path, json);
    }
}
