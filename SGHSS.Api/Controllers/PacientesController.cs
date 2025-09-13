using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGHSS.Core.DTOs;
using SGHSS.Core.Entities;
using SGHSS.Infrastructure.Data;

namespace SGHSS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PacientesController(SghssContext context, IMapper mapper) : ControllerBase
{
    // GET: api/pacientes
    [HttpGet]
    [AllowAnonymous] // 🔓
    public async Task<ActionResult<IEnumerable<PacienteDto>>> GetPacientes()
    {
        var pacientes = await context.Pacientes.ToListAsync();
        return Ok(mapper.Map<IEnumerable<PacienteDto>>(pacientes));
    }

    // GET: api/pacientes/5
    [HttpGet("{id:int}")]
    [AllowAnonymous] // 🔓
    public async Task<ActionResult<PacienteDto>> GetPaciente(int id)
    {
        var paciente = await context.Pacientes.FindAsync(id);
        if (paciente == null)
            return NotFound();

        return Ok(mapper.Map<PacienteDto>(paciente));
    }

    // POST: api/pacientes
    [HttpPost]
    [AllowAnonymous] // 🔓
    public async Task<ActionResult<PacienteDto>> CreatePaciente(CreatePacienteDto dto)
    {
        var paciente = mapper.Map<Paciente>(dto);
        context.Pacientes.Add(paciente);
        await context.SaveChangesAsync();

        var pacienteDto = mapper.Map<PacienteDto>(paciente);

        return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, pacienteDto);
    }

    // PUT: api/pacientes/5
    [HttpPut("{id:int}")]
    [Authorize] // 🔒 precisa estar logado
    public async Task<IActionResult> UpdatePaciente(int id, PacienteDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var paciente = await context.Pacientes.FindAsync(id);
        if (paciente == null)
            return NotFound();

        mapper.Map(dto, paciente);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/pacientes/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")] // 🔒 Apenas Admin pode deletar
    public async Task<IActionResult> DeletePaciente(int id)
    {
        var paciente = await context.Pacientes.FindAsync(id);
        if (paciente == null)
            return NotFound();

        context.Pacientes.Remove(paciente);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
