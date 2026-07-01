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

    public EaRepository Open(string connectionString)
    {
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
            model.RootPackages.Add(MapPackage(eaModel));
        }

        return model;
    }

    public void Close()
    {
        if (_repository != null)
        {
            _repository.CloseFile();
            _repository = null;
        }
    }

    private EaPackage MapPackage(EA.Package eaPkg)
    {
        var pkg = new EaPackage
        {
            Id = eaPkg.PackageID,
            Name = eaPkg.Name,
            Notes = eaPkg.Notes,
            ParentId = eaPkg.ParentID
        };

        if (eaPkg.Elements is EA.Collection elements)
            for (short i = 0; i < elements.Count; i++)
            {
                if (elements.GetAt(i) is EA.Element eaElem)
                    pkg.Elements.Add(MapElement(eaElem));
                else
                    _logger?.LogWarning("Unexpected type in Elements of package '{Package}' at index {Index}, skipping", pkg.Name, i);
            }

        if (eaPkg.Diagrams is EA.Collection diagrams)
            for (short i = 0; i < diagrams.Count; i++)
            {
                if (diagrams.GetAt(i) is EA.Diagram eaDiag)
                    pkg.Diagrams.Add(MapDiagram(eaDiag));
                else
                    _logger?.LogWarning("Unexpected type in Diagrams of package '{Package}' at index {Index}, skipping", pkg.Name, i);
            }

        if (eaPkg.Packages is EA.Collection packages)
            for (short i = 0; i < packages.Count; i++)
            {
                if (packages.GetAt(i) is EA.Package eaChild)
                    pkg.Children.Add(MapPackage(eaChild));
                else
                    _logger?.LogWarning("Unexpected type in Packages of package '{Package}' at index {Index}, skipping", pkg.Name, i);
            }

        return pkg;
    }

    private static EaElement MapElement(EA.Element eaElement)
    {
        var elem = new EaElement
        {
            Id = eaElement.ElementID,
            Name = eaElement.Name,
            Type = eaElement.Type,
            Stereotype = eaElement.Stereotype,
            StereotypeEx = eaElement.StereotypeEx,
            FQStereotype = eaElement.FQStereotype,
            Notes = eaElement.Notes,
            PackageId = eaElement.PackageID,
            Status = eaElement.Status ?? string.Empty,
            ModifiedDate = (DateTime)eaElement.Modified,
            CreatedDate = eaElement.Created as DateTime?
        };

        if (eaElement.Attributes is EA.Collection attrs)
            for (short i = 0; i < attrs.Count; i++)
                if (attrs.GetAt(i) is EA.Attribute eaAttr)
                    elem.Attributes.Add(new EaAttribute
                    {
                        Name = eaAttr.Name,
                        Type = eaAttr.Type,
                        Notes = eaAttr.Notes,
                        DefaultValue = eaAttr.Default
                    });

        if (eaElement.Methods is EA.Collection methods)
            for (short i = 0; i < methods.Count; i++)
                if (methods.GetAt(i) is EA.Method eaMethod)
                    elem.Methods.Add(new EaMethod
                    {
                        Name = eaMethod.Name,
                        Type = eaMethod.ReturnType,
                        Notes = eaMethod.Notes,
                        IsStatic = eaMethod.IsStatic
                    });

        if (eaElement.TaggedValues is EA.Collection taggedValues)
            for (short i = 0; i < taggedValues.Count; i++)
                if (taggedValues.GetAt(i) is EA.TaggedValue eaTv)
                    elem.TaggedValues.Add(new EaTaggedValue
                    {
                        Name = eaTv.Name,
                        Value = eaTv.Value,
                        Notes = eaTv.Notes
                    });

        if (eaElement.Connectors is EA.Collection connectors)
            for (short i = 0; i < connectors.Count; i++)
                if (connectors.GetAt(i) is EA.Connector eaConn)
                    elem.Connectors.Add(new EaConnector
                    {
                        Id = eaConn.ConnectorID,
                        Name = eaConn.Name,
                        Type = eaConn.Type,
                        Stereotype = eaConn.Stereotype,
                        Notes = eaConn.Notes,
                        SourceId = eaConn.ClientID,
                        TargetId = eaConn.SupplierID
                    });

        return elem;
    }

    private static EaDiagram MapDiagram(EA.Diagram eaDiagram)
    {
        var diagram = new EaDiagram
        {
            Id = eaDiagram.DiagramID,
            Guid = eaDiagram.DiagramGUID,
            Name = eaDiagram.Name,
            Type = eaDiagram.Type,
            Notes = eaDiagram.Notes,
            ModifiedDate = eaDiagram.ModifiedDate.ToString("yyyy-MM-dd HH:mm:ss"),
            PackageId = eaDiagram.PackageID,
        };

        if (eaDiagram.DiagramObjects is EA.Collection diagramObjects)
            for (short i = 0; i < diagramObjects.Count; i++)
                if (diagramObjects.GetAt(i) is EA.DiagramObject eaDO)
                    diagram.DiagramObjects.Add(new EaDiagramObject
                    {
                        DiagramId = eaDO.DiagramID,
                        ElementId = eaDO.ElementID,
                        Sequence = eaDO.Sequence
                    });

        return diagram;
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

    public void UpdateElementStatus(int elementId, string newStatus)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var element = _repository.GetElementByID(elementId);
        if (element == null)
            throw new InvalidOperationException($"Element {elementId} not found in repository.");
        element.Status = newStatus;
        element.Update();
        _repository.RefreshModelView(0);
        _logger?.LogInformation("Updated element {ElementId} status to '{Status}'", elementId, newStatus);
    }

    public void UpdateElementNotes(int elementId, string newNotesHtml)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var element = _repository.GetElementByID(elementId);
        if (element == null)
            throw new InvalidOperationException($"Element {elementId} not found in repository.");
        element.Notes = newNotesHtml;
        element.Update();
        _repository.RefreshModelView(0);
        _logger?.LogInformation("Updated element {ElementId} notes", elementId);
    }

    public void UpdateDiagramNotes(int diagramId, string newNotesHtml)
    {
        if (_repository == null)
            throw new InvalidOperationException("Repository is not open.");
        var diagram = _repository.GetDiagramByID(diagramId);
        if (diagram == null)
            throw new InvalidOperationException($"Diagram {diagramId} not found in repository.");
        diagram.Notes = newNotesHtml;
        diagram.Update();
        _repository.RefreshModelView(0);
        _logger?.LogInformation("Updated diagram {DiagramId} notes", diagramId);
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
        _logger?.LogInformation("Updated tagged value '{Name}' notes on element {ElementId}", tagName, elementId);
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
            Close();
            if (_repository != null)
                Marshal.ReleaseComObject(_repository);
        }
        finally
        {
            GC.SuppressFinalize(this);
        }
    }
}
