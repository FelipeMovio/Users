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
    // Exige que a policy "IdadeMinima" seja satisfeita para acessar esse endpoint.
    // O ASP.NET vai rodar todos os AuthorizationHandlers registrados que sabem
    // avaliar o requirement associado a essa policy (nesse caso, IdadeAuthorization)
}
