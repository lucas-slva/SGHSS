using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGHSS.Api.Controllers;
using SGHSS.Core.DTOs;
using SGHSS.Core.Entities;
using SGHSS.Infrastructure.Data;
using SGHSS.Infrastructure.Mappings;
using Xunit;

namespace SGHSS.Tests.Controllers;

public class ProfissionaisControllerTests
{
    private readonly ProfissionaisController _controller;
    private readonly SghssContext _context;

    public ProfissionaisControllerTests()
    {
        // Configurar DbContext InMemory
        var options = new DbContextOptionsBuilder<SghssContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new SghssContext(options);

        // Configurar AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = config.CreateMapper();

        // Instanciar controller
        _controller = new ProfissionaisController(_context, mapper);

        // Popular dados iniciais
        _context.Profissionais.Add(new Profissional
        {
            Id = 1,
            Nome = "Dr. João Souza",
            Cpf = "12345678900",
            Especialidade = "Clínico Geral"
        });
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetProfissionais_ShouldReturn_AllProfissionais()
    {
        var result = await _controller.GetProfissionais();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        if (okResult.Value is IEnumerable<ProfissionalDto> profissionais)
        {
            var profissionalDtos = profissionais as ProfissionalDto[] ?? profissionais.ToArray();
            profissionalDtos.Should().HaveCount(1);
            profissionalDtos.First().Nome.Should().Be("Dr. João Souza");
        }
    }

    [Fact]
    public async Task GetProfissional_ShouldReturn_Profissional_WhenExists()
    {
        var result = await _controller.GetProfissional(1);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        var profissional = okResult.Value as ProfissionalDto;
        profissional!.Nome.Should().Be("Dr. João Souza");
    }

    [Fact]
    public async Task GetProfissional_ShouldReturn_NotFound_WhenNotExists()
    {
        var result = await _controller.GetProfissional(99);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateProfissional_ShouldReturn_CreatedProfissional()
    {
        var dto = new CreateProfissionalDto
        {
            Nome = "Dra. Ana Paula",
            Cpf = "98765432100",
            Especialidade = "Dermatologia"
        };

        var result = await _controller.CreateProfissional(dto);

        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();

        var profissional = createdResult.Value as ProfissionalDto;
        profissional!.Nome.Should().Be("Dra. Ana Paula");
    }

    [Fact]
    public async Task UpdateProfissional_ShouldReturn_NoContent_WhenSuccessful()
    {
        var dto = new ProfissionalDto
        {
            Id = 1,
            Nome = "Dr. João Atualizado",
            Cpf = "12345678900",
            Especialidade = "Clínico Geral"
        };

        var result = await _controller.UpdateProfissional(1, dto);
        result.Should().BeOfType<NoContentResult>();

        var profissional = await _context.Profissionais.FindAsync(1);
        profissional!.Nome.Should().Be("Dr. João Atualizado");
    }

    [Fact]
    public async Task UpdateProfissional_ShouldReturn_BadRequest_WhenIdMismatch()
    {
        var dto = new ProfissionalDto
        {
            Id = 2,
            Nome = "Invalido",
            Cpf = "11122233344",
            Especialidade = "Cardiologia"
        };

        var result = await _controller.UpdateProfissional(1, dto);
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteProfissional_ShouldRemove_WhenExists()
    {
        var result = await _controller.DeleteProfissional(1);
        result.Should().BeOfType<NoContentResult>();

        var profissional = await _context.Profissionais.FindAsync(1);
        profissional.Should().BeNull();
    }

    [Fact]
    public async Task DeleteProfissional_ShouldReturn_NotFound_WhenNotExists()
    {
        var result = await _controller.DeleteProfissional(99);
        result.Should().BeOfType<NotFoundResult>();
    }
}
