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

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(
            [FromBody] CreateUsuarioDto dto)
        {
           await _usuarioService.Cadastra(dto);
            return Ok("Usuario cadastrado!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUsuarioDto dto)
        {
            await _usuarioService.Login(dto);
            return Ok("Usuario autenticado");

        }
    }
}
