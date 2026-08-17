using System.Globalization;
using EAxWiki.Core.Monitoring;

namespace EAxWiki.Monitor;

/// <summary>
/// Renders health-template.md → <c>{wikiDir}/status/health.md</c>, replacing @@TOKEN@@
/// placeholders from the HealthState. Null state values render as empty strings. The status
/// dir is a recognized special dir the exporter never cleans (InfrastructureWriter).
/// </summary>
public class HealthPageRenderer
{
    private readonly string _templatePath;
    private readonly string _outputPath;

    public HealthPageRenderer(string templatePath, string wikiDir)
    {
        _templatePath = templatePath;
        _outputPath = Path.Combine(wikiDir, "status", "health.md");
    }

    public void Render(HealthState s)
    {
        var overall = s.ConsecutiveFailures == 0 &&
                      s.ServeConsecutiveFailures == 0 &&
                      s.LlmConsecutiveFailures == 0 &&
                      s.ApiConsecutiveFailures == 0
            ? "Healthy"
            : "Degraded";

        var template = File.ReadAllText(_templatePath);
        template = Replace(template, "@@OVERALL@@", overall);
        template = Replace(template, "@@LAST_SUCCESS_TIME@@", s.LastSuccessTime);
        template = Replace(template, "@@LAST_FAILURE_TIME@@", s.LastFailureTime);
        template = Replace(template, "@@CONSECUTIVE_FAILURES@@", s.ConsecutiveFailures);
        template = Replace(template, "@@LAST_EXIT_CODE@@", s.LastExitCode);
        template = Replace(template, "@@LAST_ELEMENT_COUNT@@", s.LastElementCount);
        template = Replace(template, "@@LAST_DIAGRAM_COUNT@@", s.LastDiagramCount);
        template = Replace(template, "@@LAST_MODE@@", s.LastMode);
        template = Replace(template, "@@RUNS_SINCE_FORCE@@", s.RunsSinceForce);
        template = Replace(template, "@@LAST_SERVE_SUCCESS_TIME@@", s.LastServeSuccessTime);
        template = Replace(template, "@@LAST_SERVE_FAILURE_TIME@@", s.LastServeFailureTime);
        template = Replace(template, "@@SERVE_CONSECUTIVE_FAILURES@@", s.ServeConsecutiveFailures);
        template = Replace(template, "@@LAST_LLM_SUCCESS_TIME@@", s.LastLlmSuccessTime);
        template = Replace(template, "@@LAST_LLM_FAILURE_TIME@@", s.LastLlmFailureTime);
        template = Replace(template, "@@LLM_CONSECUTIVE_FAILURES@@", s.LlmConsecutiveFailures);
        template = Replace(template, "@@LAST_API_SUCCESS_TIME@@", s.LastApiSuccessTime);
        template = Replace(template, "@@LAST_API_FAILURE_TIME@@", s.LastApiFailureTime);
        template = Replace(template, "@@API_CONSECUTIVE_FAILURES@@", s.ApiConsecutiveFailures);

        Directory.CreateDirectory(Path.GetDirectoryName(_outputPath)!);
        File.WriteAllText(_outputPath, template);
    }

    private static string Replace(string template, string token, object? value) =>
        template.Replace(token, value is IFormattable f
            ? f.ToString(null, CultureInfo.InvariantCulture)
            : value?.ToString() ?? string.Empty);
}