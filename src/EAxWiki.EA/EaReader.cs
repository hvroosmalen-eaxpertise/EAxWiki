using System.Runtime.InteropServices;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;

namespace EAxWiki.EA;

using EA = global::EA;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class EaReader : IEaReader, IDisposable
{
    private EA.Repository? _repository;
    private string _repositoryPath = string.Empty;
    private bool _disposed;

    public string RepositoryPath => _repositoryPath;

    public EaRepository Open(string connectionString)
    {
        _repository = new EA.Repository();
        _repository.OpenFile(connectionString);
        _repositoryPath = connectionString;

        var model = new EaRepository
        {
            ConnectionString = connectionString,
            Name = connectionString
        };

        var eaModels = (EA.Collection)_repository.Models;
        for (short i = 0; i < eaModels.Count; i++)
        {
            var eaModel = (EA.Package)eaModels.GetAt(i);
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

        var elements = (EA.Collection)eaPkg.Elements;
        for (short i = 0; i < elements.Count; i++)
        {
            pkg.Elements.Add(MapElement((EA.Element)elements.GetAt(i)));
        }

        var diagrams = (EA.Collection)eaPkg.Diagrams;
        for (short i = 0; i < diagrams.Count; i++)
        {
            pkg.Diagrams.Add(MapDiagram((EA.Diagram)diagrams.GetAt(i)));
        }

        var packages = (EA.Collection)eaPkg.Packages;
        for (short i = 0; i < packages.Count; i++)
        {
            pkg.Children.Add(MapPackage((EA.Package)packages.GetAt(i)));
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
            ModifiedDate = (DateTime)eaElement.Modified,
            CreatedDate = eaElement.CreatedDate as DateTime?
        };

        var attrs = (EA.Collection)eaElement.Attributes;
        for (short i = 0; i < attrs.Count; i++)
        {
            var eaAttr = (EA.Attribute)attrs.GetAt(i);
            elem.Attributes.Add(new EaAttribute
            {
                Name = eaAttr.Name,
                Type = eaAttr.Type,
                Notes = eaAttr.Notes,
                DefaultValue = eaAttr.Default
            });
        }

        var methods = (EA.Collection)eaElement.Methods;
        for (short i = 0; i < methods.Count; i++)
        {
            var eaMethod = (EA.Method)methods.GetAt(i);
            elem.Methods.Add(new EaMethod
            {
                Name = eaMethod.Name,
                Type = eaMethod.ReturnType,
                Notes = eaMethod.Notes,
                IsStatic = eaMethod.IsStatic
            });
        }

        var taggedValues = (EA.Collection)eaElement.TaggedValues;
        for (short i = 0; i < taggedValues.Count; i++)
        {
            var eaTv = (EA.TaggedValue)taggedValues.GetAt(i);
            elem.TaggedValues.Add(new EaTaggedValue
            {
                Name = eaTv.Name,
                Value = eaTv.Value,
                Notes = eaTv.Notes
            });
        }

        var connectors = (EA.Collection)eaElement.Connectors;
        for (short i = 0; i < connectors.Count; i++)
        {
            var eaConn = (EA.Connector)connectors.GetAt(i);
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
        }

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

        var diagramObjects = (EA.Collection)eaDiagram.DiagramObjects;
        for (short i = 0; i < diagramObjects.Count; i++)
        {
            var eaDO = (EA.DiagramObject)diagramObjects.GetAt(i);
            diagram.DiagramObjects.Add(new EaDiagramObject
            {
                DiagramId = eaDO.DiagramID,
                ElementId = eaDO.ElementID,
                Sequence = eaDO.Sequence
            });
        }

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
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Close();
            if (_repository != null)
            {
                Marshal.ReleaseComObject(_repository);
            }
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
}
