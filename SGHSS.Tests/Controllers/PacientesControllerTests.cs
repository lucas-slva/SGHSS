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

public class PacientesControllerTests
{
    private readonly PacientesController _controller;
    private readonly SghssContext _context;

    public PacientesControllerTests()
    {
        // Configurar DbContext InMemory
        var options = new DbContextOptionsBuilder<SghssContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // banco único por teste
            .Options;

        _context = new SghssContext(options);

        // Configurar AutoMapper
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        var mapper = config.CreateMapper();

        // Instanciar controller
        _controller = new PacientesController(_context, mapper);

        // Popular dados iniciais
        _context.Pacientes.Add(new Paciente
        {
            Id = 1,
            Nome = "Maria da Silva",
            Cpf = "12345678901",
            DataNascimento = new DateTime(1995, 5, 20)
        });
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetPacientes_ShouldReturn_AllPacientes()
    {
        var result = await _controller.GetPacientes();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        if (okResult.Value is IEnumerable<PacienteDto> pacientes)
        {
            var pacienteDtos = pacientes as PacienteDto[] ?? pacientes.ToArray();
            pacienteDtos.Should().HaveCount(1);
            pacienteDtos.First().Nome.Should().Be("Maria da Silva");
        }
    }

    [Fact]
    public async Task GetPaciente_ShouldReturn_Paciente_WhenExists()
    {
        var result = await _controller.GetPaciente(1);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        var paciente = okResult.Value as PacienteDto;
        paciente!.Nome.Should().Be("Maria da Silva");
    }

    [Fact]
    public async Task GetPaciente_ShouldReturn_NotFound_WhenNotExists()
    {
        var result = await _controller.GetPaciente(99);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreatePaciente_ShouldReturn_CreatedPaciente()
    {
        var dto = new CreatePacienteDto
        {
            Nome = "João Souza",
            Cpf = "98765432100",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        var result = await _controller.CreatePaciente(dto);

        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();

        var paciente = createdResult.Value as PacienteDto;
        paciente!.Nome.Should().Be("João Souza");
    }

    [Fact]
    public async Task UpdatePaciente_ShouldReturn_NoContent_WhenSuccessful()
    {
        var dto = new PacienteDto
        {
            Id = 1,
            Nome = "Maria Atualizada",
            Cpf = "12345678901",
            DataNascimento = new DateTime(1995, 5, 20)
        };

        var result = await _controller.UpdatePaciente(1, dto);
        result.Should().BeOfType<NoContentResult>();

        var paciente = await _context.Pacientes.FindAsync(1);
        paciente!.Nome.Should().Be("Maria Atualizada");
    }

    [Fact]
    public async Task UpdatePaciente_ShouldReturn_BadRequest_WhenIdMismatch()
    {
        var dto = new PacienteDto
        {
            Id = 2,
            Nome = "Invalido",
            Cpf = "11122233344",
            DataNascimento = new DateTime(2000, 1, 1)
        };

        var result = await _controller.UpdatePaciente(1, dto);
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeletePaciente_ShouldRemove_WhenExists()
    {
        var result = await _controller.DeletePaciente(1);
        result.Should().BeOfType<NoContentResult>();

        var paciente = await _context.Pacientes.FindAsync(1);
        paciente.Should().BeNull();
    }

    [Fact]
    public async Task DeletePaciente_ShouldReturn_NotFound_WhenNotExists()
    {
        var result = await _controller.DeletePaciente(99);
        result.Should().BeOfType<NotFoundResult>();
    }
}
