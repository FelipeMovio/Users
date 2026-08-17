using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace UsuariosApi.Authorization;

// Handler responsável por avaliar o requirement IdadeMinima.
// AuthorizationHandler<T> é chamado automaticamente pelo ASP.NET sempre que
// uma policy que usa IdadeMinima precisa ser avaliada.

public class IdadeAuthorization :
   AuthorizationHandler<IdadeMinima>

{
    protected override Task HandleRequirementAsync
        (AuthorizationHandlerContext context, IdadeMinima requirement)
    {
        // context.User é o ClaimsPrincipal montado pelo middleware de autenticação
        // (UseAuthentication) a partir das claims contidas no JWT validado.
        // Aqui buscamos a claim de data de nascimento dentro do token.

        var dataNascimentoClaim = context
            .User.Claims.FirstOrDefault
            (claim => claim.Type == ClaimTypes.DateOfBirth);

        // Se o token não tiver essa claim, não chamamos Succeed nem Fail.
        // Resultado default do framework é FALHA — ou seja, isso já nega o acesso
        // implicitamente. Funciona, mas context.Fail(requirement) explícito
        // deixaria mais claro/rastreável o motivo da negação (melhoria opcional).

        if (dataNascimentoClaim is null)
        {
            return Task.CompletedTask;
        }

        var dataNascimento = Convert
            .ToDateTime(dataNascimentoClaim.Value);

        var idadeUsuario = 
            DateTime.Today.Year - dataNascimento.Year;

        // Ajusta a idade caso o aniversário deste ano ainda não tenha ocorrido

        if (dataNascimento > DateTime.Today.AddYears(-idadeUsuario))
            idadeUsuario--;

        // Succeed marca ESSE requirement específico como satisfeito.
        // Uma policy pode ter vários requirements (é um AND) — Succeed não
        // aprova o acesso sozinho, só sinaliza que esse requirement passou.

        if (idadeUsuario >= requirement.Idade)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
