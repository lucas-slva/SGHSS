using Microsoft.AspNetCore.Mvc;

namespace SGHSS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DebugController : ControllerBase
{
    // GET: api/debug/error
    [HttpGet("error")]
    public IActionResult GenerateError()
    {
        throw new Exception("Erro de teste gerado propositalmente no DebugController.");
    }
}