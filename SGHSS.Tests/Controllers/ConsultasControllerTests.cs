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

public class ConsultasControllerTests
{
    private readonly ConsultasController _controller;
    private readonly SghssContext _context;

    public ConsultasControllerTests()
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
        _controller = new ConsultasController(_context, mapper);

        // Popular dados iniciais
        var paciente = new Paciente { Id = 1, Nome = "Maria da Silva", Cpf = "12345678901", DataNascimento = new DateTime(1995, 5, 20) };
        var profissional = new Profissional { Id = 1, Nome = "Dr. João Souza", Cpf = "98765432100", Especialidade = "Clínico Geral" };

        _context.Pacientes.Add(paciente);
        _context.Profissionais.Add(profissional);
        _context.Consultas.Add(new Consulta
        {
            Id = 1,
            DataConsulta = DateTime.UtcNow.AddDays(7),
            Status = "Agendada",
            PacienteId = 1,
            ProfissionalId = 1
        });
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetConsultas_ShouldReturn_AllConsultas()
    {
        var result = await _controller.GetConsultas();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        if (okResult.Value is IEnumerable<ConsultaDto> consultas)
        {
            consultas.Should().HaveCount(1);
        }
    }

    [Fact]
    public async Task GetConsulta_ShouldReturn_Consulta_WhenExists()
    {
        var result = await _controller.GetConsulta(1);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();

        var consulta = okResult.Value as ConsultaDto;
        consulta!.Status.Should().Be("Agendada");
    }

    [Fact]
    public async Task GetConsulta_ShouldReturn_NotFound_WhenNotExists()
    {
        var result = await _controller.GetConsulta(99);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateConsulta_ShouldReturn_CreatedConsulta()
    {
        var dto = new CreateConsultaDto
        {
            DataConsulta = DateTime.UtcNow.AddDays(10),
            Status = "Agendada",
            PacienteId = 1,
            ProfissionalId = 1
        };

        var result = await _controller.CreateConsulta(dto);

        var createdResult = result.Result as CreatedAtActionResult;
        createdResult.Should().NotBeNull();

        var consulta = createdResult.Value as ConsultaDto;
        consulta!.Status.Should().Be("Agendada");
    }

    [Fact]
    public async Task UpdateConsulta_ShouldReturn_NoContent_WhenSuccessful()
    {
        var dto = new ConsultaDto
        {
            Id = 1,
            DataConsulta = DateTime.UtcNow.AddDays(15),
            Status = "Confirmada",
            PacienteId = 1,
            ProfissionalId = 1
        };

        var result = await _controller.UpdateConsulta(1, dto);
        result.Should().BeOfType<NoContentResult>();

        var consulta = await _context.Consultas.FindAsync(1);
        consulta!.Status.Should().Be("Confirmada");
    }

    [Fact]
    public async Task UpdateConsulta_ShouldReturn_BadRequest_WhenIdMismatch()
    {
        var dto = new ConsultaDto
        {
            Id = 2,
            DataConsulta = DateTime.UtcNow,
            Status = "Invalida",
            PacienteId = 1,
            ProfissionalId = 1
        };

        var result = await _controller.UpdateConsulta(1, dto);
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteConsulta_ShouldRemove_WhenExists()
    {
        var result = await _controller.DeleteConsulta(1);
        result.Should().BeOfType<NoContentResult>();

        var consulta = await _context.Consultas.FindAsync(1);
        consulta.Should().BeNull();
    }

    [Fact]
    public async Task DeleteConsulta_ShouldReturn_NotFound_WhenNotExists()
    {
        var result = await _controller.DeleteConsulta(99);
        result.Should().BeOfType<NotFoundResult>();
    }
}
