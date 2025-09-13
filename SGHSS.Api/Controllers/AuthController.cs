using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGHSS.Core.Services;
using SGHSS.Infrastructure.Data;

namespace SGHSS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(SghssContext context, JwtTokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var usuario = await context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == request.Email && u.SenhaHash == request.Senha);

        if (usuario == null)
            return Unauthorized("Credenciais inválidas.");

        var token = tokenService.GenerateToken(usuario);
        return Ok(new { token });
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}