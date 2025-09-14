using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGHSS.Core.DTOs;
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
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (usuario == null)
            return Unauthorized();

        // aqui ainda estamos comparando senha "plana" (depois trocamos por hash)
        if (usuario.SenhaHash != request.Senha)
            return Unauthorized();

        var token = tokenService.GenerateToken(usuario);
        return Ok(new LoginResponse { Token = token });
    }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}