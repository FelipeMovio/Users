
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsuariosApi.Models;

namespace UsuariosApi.Service;

public class TokenService
{

    public void GenerateToken(Usuario usuario)
    {
        Claim[] _claims = new Claim[]
        {
                new Claim("username", usuario.UserName),
                new Claim("id", usuario.Id),
                new Claim(ClaimTypes.DateOfBirth, usuario.DataNascimento.ToString()),

        };

        var _chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            ("dnbn237g7823rg32gr"));

        var _signingCredentials =
            new SigningCredentials
            (_chave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken
            (
            expires: DateTime.Now.AddMinutes(10),
            claims: _claims,
            signingCredentials: _signingCredentials
            );
     }
}

