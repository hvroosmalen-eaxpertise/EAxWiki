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
