namespace EAxWiki.Core.Models;

public record ExportResult(
    int TotalElements,
    int SucceededElements,
    int FailedElements,
    int DiagramsExported,
    TimeSpan Elapsed
);
