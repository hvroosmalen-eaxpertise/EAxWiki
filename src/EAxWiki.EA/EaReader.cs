using System.Runtime.InteropServices;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;
using Microsoft.Extensions.Logging;

namespace EAxWiki.EA;

using EA = global::EA;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class EaReader : IEaReader, IDisposable
{
    private EA.Repository? _repository;
    private string _repositoryPath = string.Empty;
    private bool _disposed;
    private readonly ILogger<EaReader>? _logger;

    public EaReader(ILogger<EaReader>? logger = null)
    {
        _logger = logger;
    }

    public EaReader() : this(null) { }

    public string RepositoryPath => _repositoryPath;

    // Connection strings contain '=' (e.g. "DBType=1;Connect=..."); file paths do not.
    private static bool IsConnectionString(string value) => value.Contains('=');

    public EaRepository Open(string connectionString, CancellationToken ct = default)
    {
        ct.ThrowIfCancellationRequested();

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Repository path or connection string must not be empty.", nameof(connectionString));

        if (!IsConnectionString(connectionString) && !File.Exists(connectionString))
            throw new FileNotFoundException($"EA repository file not found: {connectionString}", connectionString);

        _logger?.LogDebug("Opening EA repository with connection string: {ConnectionString}", connectionString);

        _repository = new EA.Repository();
        try
        {
            _repository.OpenFile(connectionString);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "EA COM failed to open repository '{Path}'", EaRepository.Redact(connectionString));
            throw new InvalidOperationException($"Failed to open EA repository '{EaRepository.Redact(connectionString)}': {ex.Message}", ex);
        }
        _repositoryPath = connectionString;
        ct.ThrowIfCancellationRequested();

        var model = new EaRepository
        {
            ConnectionString = connectionString,
            Name = connectionString
        };

        var eaModels = _repository.Models as EA.Collection;
        if (eaModels == null)
        {
            _logger?.LogWarning("EA repository returned no models collection");
            return model;
        }
        for (short i = 0; i < eaModels.Count; i++)
        {
            if (eaModels.GetAt(i) is not EA.Package eaModel)
            {
                _logger?.LogWarning("Unexpected type at model index {Index}, skipping", i);
                continue;
            }
            model.RootPackages.Add(ModelMapper.MapPackage(eaModel, _logger));
        }

        return model;
    }

    public bool TestConnection(string connectionString, out string? error)
    {
        EA.Repository? repo = null;
        try
        {
            repo = new EA.Repository();
            repo.OpenFile(connectionString);
            repo.CloseFile();
            error = null;
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
        finally
        {
            if (repo != null)
            {
                try
                {
                    Marshal.FinalReleaseComObject(repo);
                }
                catch (InvalidComObjectException)
                {
                    // Already released.
                }
            }
        }
    }

    public void Close()
    {
        if (_repository != null)
        {
            _repository.CloseFile();
            _repository = null;
        }
    }

    public IReadOnlyList<string> GetStatusTypes()
    {
        if (_repository == null) return [];
        var xml = _repository.SQLQuery("SELECT Status FROM t_statustypes ORDER BY Status");
        var statuses = new List<string>();
        foreach (System.Xml.Linq.XElement row in
            System.Xml.Linq.XDocument.Parse(xml)
                .Descendants("Row"))
        {
            var val = row.Element("Status")?.Value;
            if (!string.IsNullOrWhiteSpace(val))
                statuses.Add(val.Trim());
        }
        return statuses;
    }

    public string GetElementStatus(int elementId)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var element = _repository.GetElementByID(elementId);
        if (element == null)
            throw new InvalidOperationException($"Element {elementId} not found in repository.");
        return element.Status ?? string.Empty;
    }

    public EaElementSummary? GetElementSummary(int elementId)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var element = _repository.GetElementByID(elementId);
        if (element == null) return null;

        var path = new List<string>();
        var pkg = _repository.GetPackageByID(element.PackageID);
        while (pkg != null)
        {
            path.Add(pkg.Name);
            pkg = pkg.ParentID != 0 ? _repository.GetPackageByID(pkg.ParentID) : null;
        }
        path.Reverse();

        return new EaElementSummary
        {
            ElementId = element.ElementID,
            Name = element.Name,
            Type = element.Type,
            Stereotype = element.Stereotype ?? element.FQStereotype ?? string.Empty,
            PackagePath = string.Join("/", path),
            Status = element.Status ?? string.Empty,
            Notes = element.Notes,
            Attributes = MapAttributesForSummary(element),
            Methods = MapMethodsForSummary(element),
            TaggedValues = MapTaggedValuesForSummary(element),
            Relationships = MapRelationshipsForSummary(element)
        };
    }

    public EaDiagramSummary? GetDiagramSummary(int diagramId)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var diagram = _repository.GetDiagramByID(diagramId);
        if (diagram == null) return null;

        var elements = new List<DiagramElementInfo>();
        if (diagram.DiagramObjects is EA.Collection diagramObjects)
        {
            for (short i = 0; i < diagramObjects.Count; i++)
            {
                if (diagramObjects.GetAt(i) is EA.DiagramObject eaDO)
                {
                    var el = _repository.GetElementByID(eaDO.ElementID);
                    if (el != null)
                    {
                        elements.Add(new DiagramElementInfo(
                            el.Name,
                            el.Type,
                            el.Stereotype ?? el.FQStereotype ?? string.Empty,
                            el.Notes));
                    }
                }
            }
        }

        return new EaDiagramSummary
        {
            DiagramId = diagram.DiagramID,
            Name = diagram.Name,
            Type = diagram.Type,
            Notes = diagram.Notes,
            Elements = elements
        };
    }

    private static List<AttributeInfo> MapAttributesForSummary(EA.Element element)
    {
        var result = new List<AttributeInfo>();
        if (element.Attributes is EA.Collection attrs)
            for (short i = 0; i < attrs.Count; i++)
                if (attrs.GetAt(i) is EA.Attribute attr)
                    result.Add(new AttributeInfo(attr.Name, attr.Type));
        return result;
    }

    private static List<MethodInfo> MapMethodsForSummary(EA.Element element)
    {
        var result = new List<MethodInfo>();
        if (element.Methods is EA.Collection methods)
            for (short i = 0; i < methods.Count; i++)
                if (methods.GetAt(i) is EA.Method method)
                    result.Add(new MethodInfo(method.Name, method.ReturnType, method.IsStatic));
        return result;
    }

    private static List<TaggedValueInfo> MapTaggedValuesForSummary(EA.Element element)
    {
        var result = new List<TaggedValueInfo>();
        if (element.TaggedValues is EA.Collection tvs)
            for (short i = 0; i < tvs.Count; i++)
                if (tvs.GetAt(i) is EA.TaggedValue tv)
                    result.Add(new TaggedValueInfo(tv.Name, tv.Value));
        return result;
    }

    private List<RelationshipInfo> MapRelationshipsForSummary(EA.Element element)
    {
        var result = new List<RelationshipInfo>();
        if (element.Connectors is EA.Collection connectors)
        {
            for (short i = 0; i < connectors.Count; i++)
            {
                if (connectors.GetAt(i) is not EA.Connector conn) continue;
                var isSource = conn.ClientID == element.ElementID;
                var targetId = isSource ? conn.SupplierID : conn.ClientID;
                var target = _repository?.GetElementByID(targetId);
                var targetStereotype = target?.Stereotype ?? target?.FQStereotype ?? string.Empty;
                var targetNotes = target?.Notes;
                var connectorStereotype = conn.Stereotype ?? string.Empty;
                result.Add(new RelationshipInfo(
                    conn.Type,
                    isSource ? "source→target" : "target→source",
                    target?.Name ?? "(deleted)",
                    target?.Type ?? "Unknown",
                    targetStereotype,
                    targetNotes,
                    connectorStereotype));
            }
        }
        return result;
    }

    public void UpdateElementStatus(int elementId, string newStatus)
    {
        Write(
            () => _repository!.GetElementByID(elementId)
                ?? throw new InvalidOperationException($"Element {elementId} not found in repository."),
            element => { element.Status = newStatus; element.Update(); },
            () => _repository!.GetElementByID(elementId)?.Status,
            newStatus,
            $"element {elementId} Status",
            "Updated element {ElementId} status to '{Status}'",
            elementId, newStatus);
    }

    public void UpdateElementNotes(int elementId, string newNotesHtml)
    {
        Write(
            () => _repository!.GetElementByID(elementId)
                ?? throw new InvalidOperationException($"Element {elementId} not found in repository."),
            element => { element.Notes = newNotesHtml; element.Update(); },
            () => _repository!.GetElementByID(elementId)?.Notes,
            newNotesHtml,
            $"element {elementId} Notes",
            "Updated element {ElementId} notes",
            elementId);
    }

    public void UpdatePackageNotes(int packageId, string newNotesHtml)
    {
        Write(
            () => _repository!.GetPackageByID(packageId)
                ?? throw new InvalidOperationException($"Package {packageId} not found in repository."),
            package => { package.Notes = newNotesHtml; package.Update(); },
            () => _repository!.GetPackageByID(packageId)?.Notes,
            newNotesHtml,
            $"package {packageId} Notes",
            "Updated package {PackageId} notes",
            packageId);
    }

    public void UpdateDiagramNotes(int diagramId, string newNotesHtml)
    {
        Write(
            () => _repository!.GetDiagramByID(diagramId)
                ?? throw new InvalidOperationException($"Diagram {diagramId} not found in repository."),
            diagram => { diagram.Notes = newNotesHtml; diagram.Update(); },
            () => _repository!.GetDiagramByID(diagramId)?.Notes,
            newNotesHtml,
            $"diagram {diagramId} Notes",
            "Updated diagram {DiagramId} notes",
            diagramId);
    }

    // EA.Attribute/Method/TaggedValue COM objects expose no ID property (confirmed via reflection
    // on IDualAttribute/IDualMethod/IDualTaggedValue) — the parent element's collection must be
    // searched by a composite key instead. Duplicate names are legal in EA (method overloads,
    // repeated tag names), so a tie is resolved by taking the first match and logging a warning.

    public void UpdateAttributeNotes(int elementId, string attributeName, string attributeType, string newNotesHtml)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var element = _repository.GetElementByID(elementId);
        if (element == null)
            throw new InvalidOperationException($"Element {elementId} not found in repository.");
        if (element.Attributes is not EA.Collection attrs)
            throw new InvalidOperationException($"Element {elementId} has no attributes collection.");

        EA.Attribute? match = null;
        var matchCount = 0;
        for (short i = 0; i < attrs.Count; i++)
        {
            if (attrs.GetAt(i) is not EA.Attribute attr) continue;
            if (!string.Equals(attr.Name, attributeName, StringComparison.Ordinal)) continue;
            if (!string.Equals(attr.Type, attributeType, StringComparison.Ordinal)) continue;
            matchCount++;
            match ??= attr;
        }

        if (match == null)
            throw new InvalidOperationException($"Attribute '{attributeName}' ({attributeType}) not found on element {elementId}.");
        if (matchCount > 1)
            _logger?.LogWarning("Multiple attributes named '{Name}' of type '{Type}' found on element {ElementId}; updating the first match.", attributeName, attributeType, elementId);

        match.Notes = newNotesHtml;
        match.Update();
        _repository.RefreshModelView(0);
        var reElement = _repository.GetElementByID(elementId);
        string? reNotes = null;
        if (reElement?.Attributes is EA.Collection reAttrs)
            for (short j = 0; j < reAttrs.Count; j++)
                if (reAttrs.GetAt(j) is EA.Attribute reAttr &&
                    string.Equals(reAttr.Name, attributeName, StringComparison.Ordinal) &&
                    string.Equals(reAttr.Type, attributeType, StringComparison.Ordinal))
                    { reNotes = reAttr.Notes; break; }
        VerifyWrite(_logger, $"attribute '{attributeName}' ({attributeType}) on element {elementId}", newNotesHtml, reNotes);
        _logger?.LogInformation("Updated attribute '{Name}' notes on element {ElementId}", attributeName, elementId);
    }

    public void UpdateMethodNotes(int elementId, string methodName, string returnType, bool isStatic, string newNotesHtml)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var element = _repository.GetElementByID(elementId);
        if (element == null)
            throw new InvalidOperationException($"Element {elementId} not found in repository.");
        if (element.Methods is not EA.Collection methods)
            throw new InvalidOperationException($"Element {elementId} has no methods collection.");

        EA.Method? match = null;
        var matchCount = 0;
        for (short i = 0; i < methods.Count; i++)
        {
            if (methods.GetAt(i) is not EA.Method method) continue;
            if (!string.Equals(method.Name, methodName, StringComparison.Ordinal)) continue;
            if (!string.Equals(method.ReturnType, returnType, StringComparison.Ordinal)) continue;
            if (method.IsStatic != isStatic) continue;
            matchCount++;
            match ??= method;
        }

        if (match == null)
            throw new InvalidOperationException($"Method '{methodName}' ({returnType}) not found on element {elementId}.");
        if (matchCount > 1)
            _logger?.LogWarning("Multiple methods named '{Name}' ({ReturnType}) found on element {ElementId}; updating the first match.", methodName, returnType, elementId);

        match.Notes = newNotesHtml;
        match.Update();
        _repository.RefreshModelView(0);
        var reElement = _repository.GetElementByID(elementId);
        string? reNotes = null;
        if (reElement?.Methods is EA.Collection reMethods)
            for (short j = 0; j < reMethods.Count; j++)
                if (reMethods.GetAt(j) is EA.Method reMethod &&
                    string.Equals(reMethod.Name, methodName, StringComparison.Ordinal) &&
                    string.Equals(reMethod.ReturnType, returnType, StringComparison.Ordinal) &&
                    reMethod.IsStatic == isStatic)
                    { reNotes = reMethod.Notes; break; }
        VerifyWrite(_logger, $"method '{methodName}' ({returnType}) on element {elementId}", newNotesHtml, reNotes);
        _logger?.LogInformation("Updated method '{Name}' notes on element {ElementId}", methodName, elementId);
    }

    public void UpdateTaggedValueNotes(int elementId, string tagName, string tagValue, string newNotesHtml)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var element = _repository.GetElementByID(elementId);
        if (element == null)
            throw new InvalidOperationException($"Element {elementId} not found in repository.");
        if (element.TaggedValues is not EA.Collection taggedValues)
            throw new InvalidOperationException($"Element {elementId} has no tagged values collection.");

        EA.TaggedValue? match = null;
        var matchCount = 0;
        for (short i = 0; i < taggedValues.Count; i++)
        {
            if (taggedValues.GetAt(i) is not EA.TaggedValue tv) continue;
            if (!string.Equals(tv.Name, tagName, StringComparison.Ordinal)) continue;
            if (!string.Equals(tv.Value, tagValue, StringComparison.Ordinal)) continue;
            matchCount++;
            match ??= tv;
        }

        if (match == null)
            throw new InvalidOperationException($"Tagged value '{tagName}' ({tagValue}) not found on element {elementId}.");
        if (matchCount > 1)
            _logger?.LogWarning("Multiple tagged values named '{Name}' with value '{Value}' found on element {ElementId}; updating the first match.", tagName, tagValue, elementId);

        match.Notes = newNotesHtml;
        match.Update();
        _repository.RefreshModelView(0);
        var reElement = _repository.GetElementByID(elementId);
        string? reNotes = null;
        if (reElement?.TaggedValues is EA.Collection reTvs)
            for (short j = 0; j < reTvs.Count; j++)
                if (reTvs.GetAt(j) is EA.TaggedValue reTv &&
                    string.Equals(reTv.Name, tagName, StringComparison.Ordinal) &&
                    string.Equals(reTv.Value, tagValue, StringComparison.Ordinal))
                    { reNotes = reTv.Notes; break; }
        VerifyWrite(_logger, $"tagged value '{tagName}' ({tagValue}) on element {elementId}", newNotesHtml, reNotes);
        _logger?.LogInformation("Updated tagged value '{Name}' notes on element {ElementId}", tagName, elementId);
    }

    /// <summary>
    /// Shared write-back skeleton for every Update* method: null-check the open repository, locate the
    /// target entity (locate throws a descriptive exception when it cannot be found), apply the field
    /// change and COM Update, refresh the model view, re-read the value and VerifyWrite it, then log.
    /// </summary>
    private void Write<TEntity>(
        Func<TEntity> locate,
        Action<TEntity> apply,
        Func<string?> readBack,
        string expected,
        string entityDescription,
        string successTemplate,
        params object[] successArgs)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var entity = locate();
        apply(entity);
        _repository.RefreshModelView(0);
        var actual = readBack() ?? string.Empty;
        VerifyWrite(_logger, entityDescription, expected, actual);
        _logger?.LogInformation(successTemplate, successArgs);
    }

    private static void VerifyWrite(ILogger? logger, string entityDescription, string expected, string? actual)
    {
        if (string.Equals(expected, actual ?? string.Empty, StringComparison.Ordinal))
            return;

        static string Truncate(string s) => s.Length <= 200 ? s : s[..200] + "...";
        logger?.LogWarning(
            "Write-back verification failed for {Entity}: expected '{Expected}' but read back '{Actual}'",
            entityDescription, Truncate(expected), Truncate(actual ?? string.Empty));
    }

    public bool ExportDiagramImage(string diagramGuid, string filePath)
    {
        if (_repository == null) return false;
        try
        {
            var project = _repository.GetProjectInterface();
            project.PutDiagramImageToFile(diagramGuid, filePath, 1);
            return true;
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Failed to export PNG for diagram '{DiagramGuid}' to '{FilePath}'", diagramGuid, filePath);
            return false;
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        try
        {
            // Capture the RCW before Close() nulls the field, then force-release it so
            // the EA.exe -Embedding COM server sees its reference count hit zero and can
            // shut down instead of lingering as an orphan (issue #81).
            var repo = _repository;
            Close();
            if (repo != null)
            {
                try
                {
                    Marshal.FinalReleaseComObject(repo);
                }
                catch (InvalidComObjectException)
                {
                    // Already released by an earlier FinalRelease or process exit.
                }
            }
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }
}
