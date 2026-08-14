using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace UsuariosApi.Repository
{
    public class UsuarioDbContext : IdentityDbContext <Usuario>
    {

        public UsuarioDbContext
            (DbContextOptions<UsuarioDbContext> opts) : base(opts)
        {

        }
    }
}
