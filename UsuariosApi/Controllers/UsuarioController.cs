using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Dtos;
using UsuariosApi.Models;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private IMapper _mapper;
        private UserManager<Usuario> _userManager;
        public UsuarioController(IMapper mapper, UserManager<Usuario> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        } 

        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario(
            [FromBody] CreateUsuarioDto dto)
        {
            Usuario usuario = _mapper.Map<Usuario>(dto);

           IdentityResult resultado 
                = await _userManager.CreateAsync
                (usuario, dto.Password);

            if (resultado.Succeeded) {
                return Ok("Usuario cadastrado!");
            }

            throw new ApplicationException("Falha ao cadastrar");
        }
    }
}
