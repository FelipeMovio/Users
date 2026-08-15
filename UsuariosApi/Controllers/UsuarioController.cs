using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Dtos;
using UsuariosApi.Models;
using UsuariosApi.Service;

namespace UsuariosApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private UsuarioService _usuarioService;
        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        } 

        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario(
            [FromBody] CreateUsuarioDto dto)
        {
           await _usuarioService.Cadastra(dto);
            return Ok("Usuario cadastrado!");
        }
    }
}
