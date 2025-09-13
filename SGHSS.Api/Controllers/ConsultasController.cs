using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGHSS.Core.DTOs;
using SGHSS.Core.Entities;
using SGHSS.Infrastructure.Data;

namespace SGHSS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController(SghssContext context, IMapper mapper) : ControllerBase
{
    // GET: api/consultas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ConsultaDto>>> GetConsultas()
    {
        var consultas = await context.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Profissional)
            .ToListAsync();

        return Ok(mapper.Map<IEnumerable<ConsultaDto>>(consultas));
    }

    // GET: api/consultas/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ConsultaDto>> GetConsulta(int id)
    {
        var consulta = await context.Consultas
            .Include(c => c.Paciente)
            .Include(c => c.Profissional)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (consulta == null)
            return NotFound();

        return Ok(mapper.Map<ConsultaDto>(consulta));
    }

    // POST: api/consultas
    [HttpPost]
    public async Task<ActionResult<ConsultaDto>> CreateConsulta(CreateConsultaDto dto)
    {
        var consulta = mapper.Map<Consulta>(dto);

        context.Consultas.Add(consulta);
        await context.SaveChangesAsync();

        var consultaDto = mapper.Map<ConsultaDto>(consulta);

        return CreatedAtAction(nameof(GetConsulta), new { id = consulta.Id }, consultaDto);
    }

    // PUT: api/consultas/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateConsulta(int id, ConsultaDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var consulta = await context.Consultas.FindAsync(id);
        if (consulta == null)
            return NotFound();

        mapper.Map(dto, consulta);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/consultas/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteConsulta(int id)
    {
        var consulta = await context.Consultas.FindAsync(id);
        if (consulta == null)
            return NotFound();

        context.Consultas.Remove(consulta);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
