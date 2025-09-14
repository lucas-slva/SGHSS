using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using SGHSS.Core.Entities;
using SGHSS.Core.Services;

namespace SGHSS.Tests.Services;

public class JwtTokenServiceTests
{
    private readonly JwtTokenService _service;

    public JwtTokenServiceTests()
    {
        // Configuração em memória para simular appsettings.json
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"Jwt:Key", "supersecretkeysupersecretkey123456"}, // >= 32 chars
            {"Jwt:Issuer", "SGHSS"},
            {"Jwt:Audience", "SGHSSUsers"}
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _service = new JwtTokenService(config);
    }

    [Fact]
    public void GenerateToken_ShouldReturn_ValidJwtToken()
    {
        // Arrange
        var usuario = new Usuario
        {
            Id = 1,
            Nome = "Lucas Silva",
            Email = "lucas@sghss.com",
            Role = "Admin",
            SenhaHash = "hashfake"
        };

        // Act
        var token = _service.GenerateToken(usuario);

        // Assert
        token.Should().NotBeNullOrEmpty("o serviço deve retornar um token válido");
        token.Should().Contain(".");
        token.Split('.').Length.Should().Be(3, "um JWT deve conter três partes separadas por '.'");
    }
}