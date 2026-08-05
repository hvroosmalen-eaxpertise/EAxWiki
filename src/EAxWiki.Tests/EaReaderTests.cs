extern alias EAInterop;

using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EAxWiki.Tests;

#pragma warning disable CA1416

using EA = EAInterop.EA;

public class EaReaderTests
{
    #region Open validation (no COM)

    [Fact]
    public void Open_NullConnectionString_ThrowsArgument()
    {
        using var reader = new EAxWiki.EA.EaReader();
        var ex = Assert.Throws<ArgumentException>(() => reader.Open(null!));
        Assert.Contains("connectionString", ex.Message);
    }

    [Fact]
    public void Open_EmptyConnectionString_ThrowsArgument()
    {
        using var reader = new EAxWiki.EA.EaReader();
        var ex = Assert.Throws<ArgumentException>(() => reader.Open(""));
        Assert.Contains("connectionString", ex.Message);
    }

    [Fact]
    public void Open_WhitespaceConnectionString_ThrowsArgument()
    {
        using var reader = new EAxWiki.EA.EaReader();
        var ex = Assert.Throws<ArgumentException>(() => reader.Open("   "));
        Assert.Contains("connectionString", ex.Message);
    }

    [Fact]
    public void Open_NonExistentFilePath_ThrowsFileNotFound()
    {
        using var reader = new EAxWiki.EA.EaReader();
        var path = @"C:\eawiki_test_does_not_exist_" + Guid.NewGuid().ToString("N") + ".eap";
        Assert.Throws<System.IO.FileNotFoundException>(() => reader.Open(path));
    }

    #endregion

    #region MapElement

    private static Mock<EA.Collection> CreateCollection<T>(params T[] items)
        where T : class
    {
        var mock = new Mock<EA.Collection>();
        mock.Setup(c => c.Count).Returns((short)items.Length);
        for (short i = 0; i < items.Length; i++)
        {
            var idx = i;
            mock.Setup(c => c.GetAt(idx)).Returns(items[idx]);
        }
        return mock;
    }

    private static Mock<EA.Element> CreateElementMock(
        int id = 1,
        string name = "Elem",
        string type = "Class",
        string stereotype = "ESRS::Disclosure",
        string stereotypeEx = "ESRS::Disclosure",
        string fqStereotype = "ESRS::Disclosure",
        string notes = "Notes",
        int packageId = 10,
        string status = "Proposed",
        System.DateTime? modified = null,
        System.DateTime? created = null,
        EA.Collection? attributes = null,
        EA.Collection? methods = null,
        EA.Collection? taggedValues = null,
        EA.Collection? connectors = null)
    {
        var mock = new Mock<EA.Element>();
        mock.Setup(e => e.ElementID).Returns(id);
        mock.Setup(e => e.Name).Returns(name);
        mock.Setup(e => e.Type).Returns(type);
        mock.Setup(e => e.Stereotype).Returns(stereotype);
        mock.Setup(e => e.StereotypeEx).Returns(stereotypeEx);
        mock.Setup(e => e.FQStereotype).Returns(fqStereotype);
        mock.Setup(e => e.Notes).Returns(notes);
        mock.Setup(e => e.PackageID).Returns(packageId);
        mock.Setup(e => e.Status).Returns(status);
        mock.Setup(e => e.Modified).Returns(modified ?? new System.DateTime(2024, 6, 1));
        mock.Setup(e => e.Created).Returns(created ?? new System.DateTime(2023, 1, 1));
        mock.Setup(e => e.Attributes).Returns(attributes);
        mock.Setup(e => e.Methods).Returns(methods);
        mock.Setup(e => e.TaggedValues).Returns(taggedValues);
        mock.Setup(e => e.Connectors).Returns(connectors);
        return mock;
    }

    [Fact]
    public void MapElement_MapsBasicProperties()
    {
        var modified = new System.DateTime(2024, 6, 15, 10, 30, 0);
        var created = new System.DateTime(2023, 1, 1, 0, 0, 0);

        var mock = CreateElementMock(
            id: 42, name: "TestElem", type: "Class",
            stereotype: "ESRS::Disclosure", stereotypeEx: "ESRS::Disclosure",
            fqStereotype: "ESRS::Disclosure", notes: "Element notes",
            packageId: 7, status: "Approved",
            modified: modified, created: created);

        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);

        Assert.Equal(42, result.Id);
        Assert.Equal("TestElem", result.Name);
        Assert.Equal("Class", result.Type);
        Assert.Equal("ESRS::Disclosure", result.Stereotype);
        Assert.Equal("ESRS::Disclosure", result.StereotypeEx);
        Assert.Equal("ESRS::Disclosure", result.FQStereotype);
        Assert.Equal("Element notes", result.Notes);
        Assert.Equal(7, result.PackageId);
        Assert.Equal("Approved", result.Status);
        Assert.Equal(modified, result.ModifiedDate);
        Assert.Equal(created, result.CreatedDate);
    }

    [Fact]
    public void MapElement_MapsAttributes()
    {
        var attrMock1 = new Mock<EA.Attribute>();
        attrMock1.Setup(a => a.Name).Returns("Attr1");
        attrMock1.Setup(a => a.Type).Returns("int");
        attrMock1.Setup(a => a.Notes).Returns("Attr notes 1");
        attrMock1.Setup(a => a.Default).Returns("42");

        var attrMock2 = new Mock<EA.Attribute>();
        attrMock2.Setup(a => a.Name).Returns("Attr2");
        attrMock2.Setup(a => a.Type).Returns("string");
        attrMock2.Setup(a => a.Notes).Returns("Attr notes 2");
        attrMock2.Setup(a => a.Default).Returns("hello");

        var coll = CreateCollection(attrMock1.Object, attrMock2.Object);
        var mock = CreateElementMock(attributes: coll.Object);

        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);

        Assert.Equal(2, result.Attributes.Count);
        Assert.Equal("Attr1", result.Attributes[0].Name);
        Assert.Equal("int", result.Attributes[0].Type);
        Assert.Equal("Attr notes 1", result.Attributes[0].Notes);
        Assert.Equal("42", result.Attributes[0].DefaultValue);
        Assert.Equal("Attr2", result.Attributes[1].Name);
        Assert.Equal("string", result.Attributes[1].Type);
        Assert.Equal("hello", result.Attributes[1].DefaultValue);
    }

    [Fact]
    public void MapElement_MapsMethods()
    {
        var methodMock1 = new Mock<EA.Method>();
        methodMock1.Setup(m => m.Name).Returns("Foo");
        methodMock1.Setup(m => m.ReturnType).Returns("void");
        methodMock1.Setup(m => m.Notes).Returns("Method notes");
        methodMock1.Setup(m => m.IsStatic).Returns(true);

        var methodMock2 = new Mock<EA.Method>();
        methodMock2.Setup(m => m.Name).Returns("Bar");
        methodMock2.Setup(m => m.ReturnType).Returns("int");
        methodMock2.Setup(m => m.Notes).Returns("");
        methodMock2.Setup(m => m.IsStatic).Returns(false);

        var coll = CreateCollection(methodMock1.Object, methodMock2.Object);
        var mock = CreateElementMock(methods: coll.Object);

        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);

        Assert.Equal(2, result.Methods.Count);
        Assert.Equal("Foo", result.Methods[0].Name);
        Assert.Equal("void", result.Methods[0].Type);
        Assert.Equal("Method notes", result.Methods[0].Notes);
        Assert.True(result.Methods[0].IsStatic);
        Assert.Equal("Bar", result.Methods[1].Name);
        Assert.False(result.Methods[1].IsStatic);
    }

    [Fact]
    public void MapElement_MapsTaggedValues()
    {
        var tvMock1 = new Mock<EA.TaggedValue>();
        tvMock1.Setup(t => t.Name).Returns("Tag1");
        tvMock1.Setup(t => t.Value).Returns("Val1");
        tvMock1.Setup(t => t.Notes).Returns("TV notes");

        var tvMock2 = new Mock<EA.TaggedValue>();
        tvMock2.Setup(t => t.Name).Returns("Tag2");
        tvMock2.Setup(t => t.Value).Returns("Val2");
        tvMock2.Setup(t => t.Notes).Returns("");

        var coll = CreateCollection(tvMock1.Object, tvMock2.Object);
        var mock = CreateElementMock(taggedValues: coll.Object);

        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);

        Assert.Equal(2, result.TaggedValues.Count);
        Assert.Equal("Tag1", result.TaggedValues[0].Name);
        Assert.Equal("Val1", result.TaggedValues[0].Value);
        Assert.Equal("TV notes", result.TaggedValues[0].Notes);
        Assert.Equal("Tag2", result.TaggedValues[1].Name);
    }

    [Fact]
    public void MapElement_MapsConnectors()
    {
        var connMock1 = new Mock<EA.Connector>();
        connMock1.Setup(c => c.ConnectorID).Returns(100);
        connMock1.Setup(c => c.Name).Returns("Conn1");
        connMock1.Setup(c => c.Type).Returns("Association");
        connMock1.Setup(c => c.Stereotype).Returns("EA::Trace");
        connMock1.Setup(c => c.Notes).Returns("Conn notes");
        connMock1.Setup(c => c.ClientID).Returns(1);
        connMock1.Setup(c => c.SupplierID).Returns(2);

        var coll = CreateCollection(connMock1.Object);
        var mock = CreateElementMock(connectors: coll.Object);

        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);

        Assert.Single(result.Connectors);
        Assert.Equal(100, result.Connectors[0].Id);
        Assert.Equal("Conn1", result.Connectors[0].Name);
        Assert.Equal("Association", result.Connectors[0].Type);
        Assert.Equal("EA::Trace", result.Connectors[0].Stereotype);
        Assert.Equal(1, result.Connectors[0].SourceId);
        Assert.Equal(2, result.Connectors[0].TargetId);
    }

    [Fact]
    public void MapElement_NullCollections_ProducesEmptyLists()
    {
        var mock = CreateElementMock();
        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);

        Assert.Empty(result.Attributes);
        Assert.Empty(result.Methods);
        Assert.Empty(result.TaggedValues);
        Assert.Empty(result.Connectors);
    }

    [Fact]
    public void MapElement_NullStatus_ReturnsEmptyString()
    {
        var mock = CreateElementMock(status: null!);
        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);
        Assert.Equal(string.Empty, result.Status);
    }

    [Fact]
    public void MapElement_EmptyCollections_SkipsGracefully()
    {
        var coll = CreateCollection<EA.Element>();
        var mock = CreateElementMock(attributes: coll.Object, methods: coll.Object,
            taggedValues: coll.Object, connectors: coll.Object);
        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);

        Assert.Empty(result.Attributes);
        Assert.Empty(result.Methods);
        Assert.Empty(result.TaggedValues);
        Assert.Empty(result.Connectors);
    }

    [Fact]
    public void MapElement_SkipsNonMatchingItemsInCollection()
    {
        var badItem = new Mock<EA.Element>();
        var coll = CreateCollection<object>(badItem.Object);
        var mock = CreateElementMock(attributes: coll.Object);

        var result = EAxWiki.EA.ModelMapper.MapElement(mock.Object);
        Assert.Empty(result.Attributes);
    }

    #endregion

    #region MapPackage

    private static Mock<EA.Package> CreatePackageMock(
        int id = 10,
        string name = "MyPackage",
        string notes = "Pkg notes",
        int parentId = 0,
        EA.Collection? elements = null,
        EA.Collection? diagrams = null,
        EA.Collection? packages = null)
    {
        var mock = new Mock<EA.Package>();
        mock.Setup(p => p.PackageID).Returns(id);
        mock.Setup(p => p.Name).Returns(name);
        mock.Setup(p => p.Notes).Returns(notes);
        mock.Setup(p => p.ParentID).Returns(parentId);
        mock.Setup(p => p.Elements).Returns(elements);
        mock.Setup(p => p.Diagrams).Returns(diagrams);
        mock.Setup(p => p.Packages).Returns(packages);
        return mock;
    }

    [Fact]
    public void MapPackage_MapsBasicProperties()
    {
        var pkgMock = CreatePackageMock(id: 99, name: "Root", notes: "Root notes", parentId: 5);
        var result = EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, null);
        Assert.Equal(99, result.Id);
        Assert.Equal("Root", result.Name);
        Assert.Equal("Root notes", result.Notes);
        Assert.Equal(5, result.ParentId);
    }

    [Fact]
    public void MapPackage_MapsElements()
    {
        var elemMock = CreateElementMock(id: 1, name: "Child");
        var coll = CreateCollection(elemMock.Object);
        var pkgMock = CreatePackageMock(elements: coll.Object);
        var result = EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, null);
        Assert.Single(result.Elements);
        Assert.Equal(1, result.Elements[0].Id);
    }

    [Fact]
    public void MapPackage_MapsDiagrams()
    {
        var diagMock = new Mock<EA.Diagram>();
        diagMock.Setup(d => d.DiagramID).Returns(55);
        diagMock.Setup(d => d.DiagramGUID).Returns("{GUID}");
        diagMock.Setup(d => d.Name).Returns("MyDiagram");
        diagMock.Setup(d => d.Type).Returns("Logical");
        diagMock.Setup(d => d.Notes).Returns("Diagram notes");
        diagMock.Setup(d => d.ModifiedDate).Returns(new System.DateTime(2024, 1, 1));
        diagMock.Setup(d => d.PackageID).Returns(10);

        var coll = CreateCollection(diagMock.Object);
        var pkgMock = CreatePackageMock(diagrams: coll.Object);
        var result = EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, null);
        Assert.Single(result.Diagrams);
        Assert.Equal(55, result.Diagrams[0].Id);
        Assert.Equal("MyDiagram", result.Diagrams[0].Name);
    }

    [Fact]
    public void MapPackage_MapsChildPackages()
    {
        var childMock = CreatePackageMock(id: 20, name: "Child", parentId: 10);
        var coll = CreateCollection(childMock.Object);
        var pkgMock = CreatePackageMock(packages: coll.Object);
        var result = EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, null);
        Assert.Single(result.Children);
        Assert.Equal(20, result.Children[0].Id);
        Assert.Equal("Child", result.Children[0].Name);
    }

    [Fact]
    public void MapPackage_NullCollections_ProducesEmptyLists()
    {
        var pkgMock = CreatePackageMock();
        var result = EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, null);
        Assert.Empty(result.Elements);
        Assert.Empty(result.Diagrams);
        Assert.Empty(result.Children);
    }

    [Fact]
    public void MapPackage_RecursiveNestedPackages()
    {
        var grandchildMock = CreatePackageMock(id: 30, name: "Grandchild", parentId: 20);
        var gcColl = CreateCollection(grandchildMock.Object);
        var childMock = CreatePackageMock(id: 20, name: "Child", parentId: 10, packages: gcColl.Object);
        var childColl = CreateCollection(childMock.Object);
        var rootMock = CreatePackageMock(id: 10, name: "Root", packages: childColl.Object);
        var result = EAxWiki.EA.ModelMapper.MapPackage(rootMock.Object, null);
        Assert.Single(result.Children);
        Assert.Equal(20, result.Children[0].Id);
        Assert.Single(result.Children[0].Children);
        Assert.Equal(30, result.Children[0].Children[0].Id);
    }

    [Fact]
    public void MapPackage_WithLogger_WarnsOnUnexpectedElementType()
    {
        var loggerMock = new Mock<ILogger>();
        var coll = CreateCollection<object>(new object());
        var pkgMock = CreatePackageMock(elements: coll.Object);

        EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, loggerMock.Object);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void MapPackage_WithLogger_WarnsOnUnexpectedDiagramType()
    {
        var loggerMock = new Mock<ILogger>();
        var coll = CreateCollection<object>(new object());
        var pkgMock = CreatePackageMock(diagrams: coll.Object);

        EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, loggerMock.Object);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void MapPackage_WithLogger_WarnsOnUnexpectedPackageType()
    {
        var loggerMock = new Mock<ILogger>();
        var coll = CreateCollection<object>(new object());
        var pkgMock = CreatePackageMock(packages: coll.Object);

        EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, loggerMock.Object);

        loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public void MapPackage_WithLogger_NoWarningsOnValidInput()
    {
        var loggerMock = new Mock<ILogger>(MockBehavior.Strict);
        var elemMock = CreateElementMock(id: 1);
        var elemColl = CreateCollection(elemMock.Object);
        var diagMock = new Mock<EA.Diagram>();
        diagMock.Setup(d => d.DiagramID).Returns(1);
        diagMock.Setup(d => d.DiagramGUID).Returns("{G}");
        diagMock.Setup(d => d.Name).Returns("D");
        diagMock.Setup(d => d.Type).Returns("Logical");
        diagMock.Setup(d => d.Notes).Returns("");
        diagMock.Setup(d => d.ModifiedDate).Returns(new System.DateTime(2024, 1, 1));
        diagMock.Setup(d => d.PackageID).Returns(10);
        var diagColl = CreateCollection(diagMock.Object);
        var childMock = CreatePackageMock(id: 20, name: "Child", parentId: 10);
        var childColl = CreateCollection(childMock.Object);
        var pkgMock = CreatePackageMock(elements: elemColl.Object, diagrams: diagColl.Object, packages: childColl.Object);

        EAxWiki.EA.ModelMapper.MapPackage(pkgMock.Object, loggerMock.Object);
    }

    #endregion

    #region MapDiagram

    [Fact]
    public void MapDiagram_MapsBasicProperties()
    {
        var diagMock = new Mock<EA.Diagram>();
        diagMock.Setup(d => d.DiagramID).Returns(77);
        diagMock.Setup(d => d.DiagramGUID).Returns("{GUID-123}");
        diagMock.Setup(d => d.Name).Returns("Architecture");
        diagMock.Setup(d => d.Type).Returns("Class");
        diagMock.Setup(d => d.Notes).Returns("Diagram description");
        diagMock.Setup(d => d.ModifiedDate).Returns(new System.DateTime(2024, 6, 1));
        diagMock.Setup(d => d.PackageID).Returns(10);

        var result = EAxWiki.EA.ModelMapper.MapDiagram(diagMock.Object);

        Assert.Equal(77, result.Id);
        Assert.Equal("{GUID-123}", result.Guid);
        Assert.Equal("Architecture", result.Name);
        Assert.Equal("Class", result.Type);
        Assert.Equal("Diagram description", result.Notes);
        Assert.Equal("2024-06-01 00:00:00", result.ModifiedDate);
        Assert.Equal(10, result.PackageId);
    }

    [Fact]
    public void MapDiagram_MapsDiagramObjects()
    {
        var doMock = new Mock<EA.DiagramObject>();
        doMock.Setup(d => d.DiagramID).Returns(77);
        doMock.Setup(d => d.ElementID).Returns(42);
        doMock.Setup(d => d.Sequence).Returns(1);

        var coll = CreateCollection(doMock.Object);
        var diagMock = new Mock<EA.Diagram>();
        diagMock.Setup(d => d.DiagramID).Returns(77);
        diagMock.Setup(d => d.DiagramGUID).Returns("{G}");
        diagMock.Setup(d => d.Name).Returns("D");
        diagMock.Setup(d => d.Type).Returns("Logical");
        diagMock.Setup(d => d.Notes).Returns("");
        diagMock.Setup(d => d.ModifiedDate).Returns(new System.DateTime(2024, 1, 1));
        diagMock.Setup(d => d.PackageID).Returns(10);
        diagMock.Setup(d => d.DiagramObjects).Returns(coll.Object);

        var result = EAxWiki.EA.ModelMapper.MapDiagram(diagMock.Object);

        Assert.Single(result.DiagramObjects);
        Assert.Equal(77, result.DiagramObjects[0].DiagramId);
        Assert.Equal(42, result.DiagramObjects[0].ElementId);
        Assert.Equal(1, result.DiagramObjects[0].Sequence);
    }

    [Fact]
    public void MapDiagram_NullDiagramObjects_ProducesEmptyList()
    {
        var diagMock = new Mock<EA.Diagram>();
        diagMock.Setup(d => d.DiagramID).Returns(1);
        diagMock.Setup(d => d.DiagramGUID).Returns("{G}");
        diagMock.Setup(d => d.Name).Returns("D");
        diagMock.Setup(d => d.Type).Returns("Logical");
        diagMock.Setup(d => d.Notes).Returns("");
        diagMock.Setup(d => d.ModifiedDate).Returns(new System.DateTime(2024, 1, 1));
        diagMock.Setup(d => d.PackageID).Returns(1);
        diagMock.Setup(d => d.DiagramObjects).Returns((EA.Collection?)null!);

        var result = EAxWiki.EA.ModelMapper.MapDiagram(diagMock.Object);

        Assert.Empty(result.DiagramObjects);
    }

    #endregion

    #region Guard clause tests (no COM)

    [Fact]
    public void GetElementStatus_WhenNotOpen_Throws()
    {
        using var reader = new EAxWiki.EA.EaReader();
        var ex = Assert.Throws<System.InvalidOperationException>(() => reader.GetElementStatus(1));
        Assert.Contains("not open", ex.Message);
    }

    [Fact]
    public void UpdateElementStatus_WhenNotOpen_Throws()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.Throws<System.InvalidOperationException>(() => reader.UpdateElementStatus(1, "Proposed"));
    }

    [Fact]
    public void UpdateElementNotes_WhenNotOpen_Throws()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.Throws<System.InvalidOperationException>(() => reader.UpdateElementNotes(1, "<p>html</p>"));
    }

    [Fact]
    public void UpdateDiagramNotes_WhenNotOpen_Throws()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.Throws<System.InvalidOperationException>(() => reader.UpdateDiagramNotes(1, "<p>html</p>"));
    }

    [Fact]
    public void UpdateAttributeNotes_WhenNotOpen_Throws()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.Throws<System.InvalidOperationException>(() => reader.UpdateAttributeNotes(1, "attr", "int", "<p>html</p>"));
    }

    [Fact]
    public void UpdateMethodNotes_WhenNotOpen_Throws()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.Throws<System.InvalidOperationException>(() => reader.UpdateMethodNotes(1, "method", "void", false, "<p>html</p>"));
    }

    [Fact]
    public void UpdateTaggedValueNotes_WhenNotOpen_Throws()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.Throws<System.InvalidOperationException>(() => reader.UpdateTaggedValueNotes(1, "tag", "val", "<p>html</p>"));
    }

    [Fact]
    public void ExportDiagramImage_WhenNotOpen_ReturnsFalse()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.False(reader.ExportDiagramImage("{guid}", "out.png"));
    }

    [Fact]
    public void Close_WhenNotOpen_DoesNotThrow()
    {
        var reader = new EAxWiki.EA.EaReader();
        reader.Close();
    }

    [Fact]
    public void RepositoryPath_WhenNotOpen_IsEmpty()
    {
        using var reader = new EAxWiki.EA.EaReader();
        Assert.Equal(string.Empty, reader.RepositoryPath);
    }

    #endregion

    #region Dispose

    [Fact]
    public void Dispose_CalledMultipleTimes_DoesNotThrow()
    {
        var reader = new EAxWiki.EA.EaReader();
        reader.Dispose();
        reader.Dispose();
    }

    [Fact]
    public void Dispose_AfterClose_DoesNotThrow()
    {
        // Regression (issue #81): Dispose used to call Close() first, which nulled
        // _repository, making the ReleaseComObject branch dead code. The fixed path
        // captures the RCW before Close(); a not-open reader must still no-op cleanly.
        var reader = new EAxWiki.EA.EaReader();
        reader.Close();
        reader.Dispose();
    }

    #endregion
}
