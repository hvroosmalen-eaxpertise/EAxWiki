using System.Runtime.InteropServices;
using EAxWiki.Core.Interfaces;
using EAxWiki.Core.Models;

namespace EAxWiki.EA;

[System.Runtime.Versioning.SupportedOSPlatform("windows")]
public class EaReader : IEaReader, IDisposable
{
    private dynamic? _repository;
    private bool _disposed;

    public EaRepository Open(string connectionString)
    {
        var repoType = Type.GetTypeFromProgID("EA.Repository")
            ?? throw new InvalidOperationException("EA.Repository COM type not found. Ensure Enterprise Architect is installed.");

        _repository = Activator.CreateInstance(repoType)
            ?? throw new InvalidOperationException("Failed to create EA.Repository instance.");
        _repository.Open(connectionString);

        var model = new EaRepository
        {
            ConnectionString = connectionString,
            Name = connectionString
        };

        dynamic models = _repository.Models;
        int count = models.Count;

        for (int i = 0; i < count; i++)
        {
            dynamic eaModel = models.GetAt((short)i);
            var rootPkg = eaModel.Package;
            model.RootPackages.Add(MapPackage(rootPkg));
        }

        return model;
    }

    public void Close()
    {
        if (_repository != null)
        {
            _repository.Close();
            _repository = null;
        }
    }

    private EaPackage MapPackage(dynamic eaPkg)
    {
        var pkg = new EaPackage
        {
            Id = eaPkg.PackageID,
            Name = eaPkg.Name,
            Notes = eaPkg.Notes,
            ParentId = eaPkg.ParentID
        };

        dynamic elements = eaPkg.Elements;
        int elemCount = elements.Count;
        for (int i = 0; i < elemCount; i++)
        {
            pkg.Elements.Add(MapElement(elements.GetAt((short)i)));
        }

        dynamic diagrams = eaPkg.Diagrams;
        int diagCount = diagrams.Count;
        for (int i = 0; i < diagCount; i++)
        {
            pkg.Diagrams.Add(MapDiagram(diagrams.GetAt((short)i)));
        }

        dynamic packages = eaPkg.Packages;
        int childCount = packages.Count;
        for (int i = 0; i < childCount; i++)
        {
            pkg.Children.Add(MapPackage(packages.GetAt((short)i)));
        }

        return pkg;
    }

    private EaElement MapElement(dynamic eaElement)
    {
        var elem = new EaElement
        {
            Id = eaElement.ElementID,
            Name = eaElement.Name,
            Type = eaElement.Type,
            Stereotype = eaElement.Stereotype,
            Notes = eaElement.Notes,
            PackageId = eaElement.PackageID
        };

        dynamic attrs = eaElement.Attributes;
        int attrCount = attrs.Count;
        for (int i = 0; i < attrCount; i++)
        {
            dynamic eaAttr = attrs.GetAt((short)i);
            elem.Attributes.Add(new EaAttribute
            {
                Name = eaAttr.Name,
                Type = eaAttr.Type,
                Notes = eaAttr.Notes,
                DefaultValue = eaAttr.Default
            });
        }

        dynamic methods = eaElement.Methods;
        int methodCount = methods.Count;
        for (int i = 0; i < methodCount; i++)
        {
            dynamic eaMethod = methods.GetAt((short)i);
            elem.Methods.Add(new EaMethod
            {
                Name = eaMethod.Name,
                Type = eaMethod.ReturnType,
                Notes = eaMethod.Notes,
                IsStatic = eaMethod.IsStatic
            });
        }

        dynamic taggedValues = eaElement.TaggedValues;
        int tvCount = taggedValues.Count;
        for (int i = 0; i < tvCount; i++)
        {
            dynamic eaTv = taggedValues.GetAt((short)i);
            elem.TaggedValues.Add(new EaTaggedValue
            {
                Name = eaTv.Name,
                Value = eaTv.Value,
                Notes = eaTv.Notes
            });
        }

        dynamic connectors = eaElement.Connectors;
        int connCount = connectors.Count;
        for (int i = 0; i < connCount; i++)
        {
            dynamic eaConn = connectors.GetAt((short)i);
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

    private static EaDiagram MapDiagram(dynamic eaDiagram)
    {
        return new EaDiagram
        {
            Id = eaDiagram.DiagramID,
            Name = eaDiagram.Name,
            Type = eaDiagram.Type,
            Notes = eaDiagram.Notes,
            PackageId = eaDiagram.PackageID
        };
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
