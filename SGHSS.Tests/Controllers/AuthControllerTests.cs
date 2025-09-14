using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SGHSS.Api.Controllers;
using SGHSS.Core.DTOs;
using SGHSS.Core.Entities;
using SGHSS.Core.Services;
using SGHSS.Infrastructure.Data;
using Xunit;

namespace SGHSS.Tests.Controllers;

public class AuthControllerTests
{
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        // Configurar DbContext InMemory
        var options = new DbContextOptionsBuilder<SghssContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new SghssContext(options);

        // Adicionar usuário de teste
        context.Usuarios.Add(new Usuario
        {
            Id = 1,
            Nome = "Teste User",
            Email = "teste@sghss.com",
            SenhaHash = "123456", // ainda sem hash real
            Role = "User"
        });
        context.SaveChanges();

        // Configurar serviço de token com chave fixa
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "Jwt:Key", "super_secret_key_super_secret_key" },
                { "Jwt:Issuer", "sghss" },
                { "Jwt:Audience", "sghss_users" }
            }!)
            .Build();

        var tokenService = new JwtTokenService(config);

        _controller = new AuthController(context, tokenService);
    }

    [Fact]
    public async Task Login_ShouldReturn_Token_WhenCredentialsAreValid()
    {
        var request = new LoginRequest
        {
            Email = "teste@sghss.com",
            Senha = "123456"
        };

        var result = await _controller.Login(request);

        var okResult = result as OkObjectResult;
        okResult.Should().NotBeNull();

        var response = okResult!.Value as LoginResponse;
        response.Should().NotBeNull();
        response!.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Login_ShouldReturn_Unauthorized_WhenPasswordIsInvalid()
    {
        var request = new LoginRequest
        {
            Email = "teste@sghss.com",
            Senha = "wrongpass"
        };

        var result = await _controller.Login(request);

        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Login_ShouldReturn_Unauthorized_WhenUserDoesNotExist()
    {
        var request = new LoginRequest
        {
            Email = "naoexiste@sghss.com",
            Senha = "123456"
        };

        var result = await _controller.Login(request);

        result.Should().BeOfType<UnauthorizedResult>();
    }
}
