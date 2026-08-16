using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsuariosApi.Controllers;

[ApiController]
[Route("{controller}")]
public class AcessoController : Controller
{
    [HttpGet]
    [Authorize(Policy ="IdadeMinima")]
    public IActionResult Get()
    {
        return Ok("Acesso permitido");
    }
}
