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
public class ProfissionaisController(SghssContext context, IMapper mapper) : ControllerBase
{
    // GET: api/profissionais
    [HttpGet]
    [AllowAnonymous] // 🔓
    public async Task<ActionResult<IEnumerable<ProfissionalDto>>> GetProfissionais()
    {
        var profissionais = await context.Profissionais.ToListAsync();
        return Ok(mapper.Map<IEnumerable<ProfissionalDto>>(profissionais));
    }

    // GET: api/profissionais/5
    [HttpGet("{id:int}")]
    [AllowAnonymous] // 🔓
    public async Task<ActionResult<ProfissionalDto>> GetProfissional(int id)
    {
        var profissional = await context.Profissionais.FindAsync(id);
        if (profissional == null)
            return NotFound();

        return Ok(mapper.Map<ProfissionalDto>(profissional));
    }

    // POST: api/profissionais
    [HttpPost]
    [AllowAnonymous] // 🔓
    public async Task<ActionResult<ProfissionalDto>> CreateProfissional(CreateProfissionalDto dto)
    {
        var profissional = mapper.Map<Profissional>(dto);
        context.Profissionais.Add(profissional);
        await context.SaveChangesAsync();

        var profissionalDto = mapper.Map<ProfissionalDto>(profissional);

        return CreatedAtAction(nameof(GetProfissional), new { id = profissional.Id }, profissionalDto);
    }

    // PUT: api/profissionais/5
    [HttpPut("{id:int}")]
    [Authorize] // 🔒 precisa estar autenticado
    public async Task<IActionResult> UpdateProfissional(int id, ProfissionalDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var profissional = await context.Profissionais.FindAsync(id);
        if (profissional == null)
            return NotFound();

        mapper.Map(dto, profissional);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/profissionais/5
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")] // 🔒 Apenas Admin pode deletar
    public async Task<IActionResult> DeleteProfissional(int id)
    {
        var profissional = await context.Profissionais.FindAsync(id);
        if (profissional == null)
            return NotFound();

        context.Profissionais.Remove(profissional);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
