using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsuariosApi.Dtos;
using UsuariosApi.Models;

namespace UsuariosApi.Service
{
    public class UsuarioService
    {
        private IMapper _mapper;
        private UserManager<Usuario> _userManager;
        private SignInManager <Usuario> _singInMenager;
        private TokenService _tokenService;
        public UsuarioService(IMapper mapper, UserManager<Usuario> userManager,
            SignInManager<Usuario> singInMenager,TokenService tokenService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _singInMenager = singInMenager;
            _tokenService = tokenService;
        }


        public async Task Cadastra(CreateUsuarioDto dto)
        {
            Usuario usuario = _mapper.Map<Usuario>(dto);

            IdentityResult resultado
                 = await _userManager.CreateAsync
                 (usuario, dto.Password);

            if (!resultado.Succeeded)
            {
                throw new ApplicationException("Falha ao cadastrar");
            }
        }

        internal async Task Login(LoginUsuarioDto dto)
        {
            var resultado = 
                await _singInMenager.PasswordSignInAsync
                (dto.Username, dto.Password,false,false);

            if (!resultado.Succeeded)
            {
                throw new ApplicationException
                    ("usuario nao autenticado");
            }

            _tokenService.GenerateToken();
        }
    }
}
