using AutoMapper;
using UsuariosApi.Dtos;
using UsuariosApi.Miodels;

namespace UsuariosApi.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<CreateUsuarioDto, Usuario>();
    }
}
