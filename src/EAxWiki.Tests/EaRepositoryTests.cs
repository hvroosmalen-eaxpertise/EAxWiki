using EAxWiki.Core.Models;

namespace EAxWiki.Tests;

public class EaRepositoryTests
{
    [Fact]
    public void Redact_NullInput_ReturnsEmptyString()
    {
        var result = EaRepository.Redact(null!);
        Assert.Equal("", result);
    }

    [Fact]
    public void Redact_NoEquals_ReturnsUnchanged()
    {
        Assert.Equal("plaintext", EaRepository.Redact("plaintext"));
    }

    [Fact]
    public void Redact_PasswordField_ReplacedWithAsterisks()
    {
        Assert.Equal("Data Source=db;Password=***;", EaRepository.Redact("Data Source=db;Password=secret123;"));
    }

    [Fact]
    public void Redact_PwdField_ReplacedWithAsterisks()
    {
        Assert.Equal("Data Source=db;Pwd=***;", EaRepository.Redact("Data Source=db;Pwd=secret;"));
    }

    [Fact]
    public void Redact_UserIdField_ReplacedWithAsterisks()
    {
        Assert.Equal("Server=s;User Id=***;", EaRepository.Redact("Server=s;User Id=admin;"));
    }

    [Fact]
    public void Redact_UidField_ReplacedWithAsterisks()
    {
        Assert.Equal("Server=s;Uid=***;", EaRepository.Redact("Server=s;Uid=admin;"));
    }

    [Fact]
    public void Redact_UserNameField_ReplacedWithAsterisks()
    {
        Assert.Equal("Server=s;User Name=***;", EaRepository.Redact("Server=s;User Name=admin;"));
    }

    [Fact]
    public void Redact_UsernameField_ReplacedWithAsterisks()
    {
        Assert.Equal("Server=s;Username=***;", EaRepository.Redact("Server=s;Username=admin;"));
    }

    [Fact]
    public void Redact_CaseInsensitive_HandlesAllCases()
    {
        Assert.Equal("PASSWORD=***;password=***;Pwd=***;", EaRepository.Redact("PASSWORD=secret;password=secret;Pwd=secret;"));
    }

    [Fact]
    public void Redact_MultipleSensitiveFields_AllRedacted()
    {
        Assert.Equal("Data Source=db;User Id=***;Password=***;", EaRepository.Redact("Data Source=db;User Id=admin;Password=secret;"));
    }

    [Fact]
    public void Redact_EmptyValueAfterEquals_ReplacesEmpty()
    {
        Assert.Equal("Password=***;", EaRepository.Redact("Password=;"));
    }

    [Fact]
    public void Redact_NoSensitiveFields_Unchanged()
    {
        Assert.Equal("Data Source=db;Initial Catalog=mine;", EaRepository.Redact("Data Source=db;Initial Catalog=mine;"));
    }

    [Fact]
    public void Redact_ValueContainsEquals_RedactsCorrectly()
    {
        Assert.Equal("Password=***;", EaRepository.Redact("Password=foo=bar;"));
    }
}
