using EAxWiki.Core.Models;
using Microsoft.Extensions.Logging;

namespace EAxWiki.EA;

using EA = global::EA;

internal static class ModelMapper
{
    internal static EaPackage MapPackage(EA.Package eaPkg, ILogger? logger = null)
    {
        var pkg = new EaPackage
        {
            Id = eaPkg.PackageID,
            Name = eaPkg.Name,
            Notes = eaPkg.Notes,
            ParentId = eaPkg.ParentID
        };

        if (eaPkg.Elements is EA.Collection elementsColl)
        {
            var matched = 0;
            foreach (var eaElem in elementsColl.ToEnumerable<EA.Element>())
            {
                pkg.Elements.Add(MapElement(eaElem));
                matched++;
            }
            if (matched < elementsColl.Count)
                logger?.LogWarning("Unexpected type(s) in Elements of package '{Package}', skipped {Skipped} entries", pkg.Name, elementsColl.Count - matched);
        }

        if (eaPkg.Diagrams is EA.Collection diagramsColl)
        {
            var matched = 0;
            foreach (var eaDiag in diagramsColl.ToEnumerable<EA.Diagram>())
            {
                pkg.Diagrams.Add(MapDiagram(eaDiag));
                matched++;
            }
            if (matched < diagramsColl.Count)
                logger?.LogWarning("Unexpected type(s) in Diagrams of package '{Package}', skipped {Skipped} entries", pkg.Name, diagramsColl.Count - matched);
        }

        if (eaPkg.Packages is EA.Collection packagesColl)
        {
            var matched = 0;
            foreach (var eaChild in packagesColl.ToEnumerable<EA.Package>())
            {
                pkg.Children.Add(MapPackage(eaChild, logger));
                matched++;
            }
            if (matched < packagesColl.Count)
                logger?.LogWarning("Unexpected type(s) in Packages of package '{Package}', skipped {Skipped} entries", pkg.Name, packagesColl.Count - matched);
        }

        return pkg;
    }

    internal static EaElement MapElement(EA.Element eaElement)
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
            ModifiedDate = eaElement.Modified as DateTime? ?? DateTime.MinValue,
            CreatedDate = eaElement.Created as DateTime?
        };

        foreach (var eaAttr in eaElement.Attributes.ToEnumerable<EA.Attribute>())
            elem.Attributes.Add(new EaAttribute
            {
                Name = eaAttr.Name,
                Type = eaAttr.Type,
                Notes = eaAttr.Notes,
                DefaultValue = eaAttr.Default
            });

        foreach (var eaMethod in eaElement.Methods.ToEnumerable<EA.Method>())
            elem.Methods.Add(new EaMethod
            {
                Name = eaMethod.Name,
                Type = eaMethod.ReturnType,
                Notes = eaMethod.Notes,
                IsStatic = eaMethod.IsStatic
            });

        foreach (var eaTv in eaElement.TaggedValues.ToEnumerable<EA.TaggedValue>())
            elem.TaggedValues.Add(new EaTaggedValue
            {
                Name = eaTv.Name,
                Value = eaTv.Value,
                Notes = eaTv.Notes
            });

        foreach (var eaConn in eaElement.Connectors.ToEnumerable<EA.Connector>())
            elem.Connectors.Add(new EaConnector
            {
                Id = eaConn.ConnectorID,
                Name = eaConn.Name,
                Type = eaConn.Type,
                Stereotype = eaConn.Stereotype,
                StereotypeEx = eaConn.StereotypeEx,
                FQStereotype = eaConn.FQStereotype,
                Notes = eaConn.Notes,
                SourceId = eaConn.ClientID,
                TargetId = eaConn.SupplierID
            });

        return elem;
    }

    internal static EaDiagram MapDiagram(EA.Diagram eaDiagram)
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

        foreach (var eaDO in eaDiagram.DiagramObjects.ToEnumerable<EA.DiagramObject>())
            diagram.DiagramObjects.Add(new EaDiagramObject
            {
                DiagramId = eaDO.DiagramID,
                ElementId = eaDO.ElementID,
                Sequence = eaDO.Sequence
            });

        return diagram;
    }
}
