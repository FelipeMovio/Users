using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Dtos;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        [HttpPost]
        public IActionResult CadastrarUsuario(
            [FromBody] CreateUsuarioDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
