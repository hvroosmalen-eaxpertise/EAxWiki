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
            Status = eaPkg.Status ?? string.Empty,
            Notes = eaPkg.Notes,
            ParentId = eaPkg.ParentID
        };

        if (eaPkg.Elements is EA.Collection elements)
            for (short i = 0; i < elements.Count; i++)
            {
                if (elements.GetAt(i) is EA.Element eaElem)
                    pkg.Elements.Add(MapElement(eaElem));
                else
                    logger?.LogWarning("Unexpected type in Elements of package '{Package}' at index {Index}, skipping", pkg.Name, i);
            }

        if (eaPkg.Diagrams is EA.Collection diagrams)
            for (short i = 0; i < diagrams.Count; i++)
            {
                if (diagrams.GetAt(i) is EA.Diagram eaDiag)
                    pkg.Diagrams.Add(MapDiagram(eaDiag));
                else
                    logger?.LogWarning("Unexpected type in Diagrams of package '{Package}' at index {Index}, skipping", pkg.Name, i);
            }

        if (eaPkg.Packages is EA.Collection packages)
            for (short i = 0; i < packages.Count; i++)
            {
                if (packages.GetAt(i) is EA.Package eaChild)
                    pkg.Children.Add(MapPackage(eaChild, logger));
                else
                    logger?.LogWarning("Unexpected type in Packages of package '{Package}' at index {Index}, skipping", pkg.Name, i);
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
}
