using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;
using UsuariosApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Lê a connection string do MySQL (appsettings) e registra o UsuarioDbContext no DI,
// usando o provider MySQL (Pomelo). AutoDetect conecta no banco no startup pra
// identificar a versão — se preferir não depender de conexão ativa na inicialização,
// trocar por ServerVersion.Create(...) com a versão fixa.
var connectionString = builder.Configuration.GetConnectionString("UsuarioApiConnection");

builder.Services
    .AddDbContext<UsuarioDbContext>(opts =>
    opts.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Registra o ASP.NET Core Identity para gerenciar usuários (Usuario) e roles (IdentityRole):
// hash de senha, criação/validação de usuário, roles, lockout, etc.
// - AddEntityFrameworkStores: persiste tudo via UsuarioDbContext (EF Core).
// - AddDefaultTokenProviders: habilita tokens para reset de senha, confirmação de email e 2FA.
// OBS: isso gerencia usuários/senhas, não emite JWT — se o login precisar de JWT,
// isso ainda precisa ser implementado separadamente após a autenticação do Identity.
builder.Services
    .AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<UsuarioDbContext>()
    .AddDefaultTokenProviders();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
