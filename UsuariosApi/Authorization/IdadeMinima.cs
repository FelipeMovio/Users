using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace UsuariosApi.Authorization;

public class IdadeMinima : IAuthorizationRequirement
{
    public int Idade { get; set; }

    public IdadeMinima(int idade)
    {
        Idade = idade;
    }

    //IAuthorizationRequirement é só um marcador — uma interface vazia(sem métodos).
    //O Requirement carrega dados de configuração(nesse caso, a idade mínima exigida),
    //mas não tem lógica nenhuma.Pensa nele como um "parâmetro de política":
    //você registra new IdadeMinima(18),e esse 18 fica disponível pro handler decidir o que fazer.

}
