using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UsuariosApi.Authorization;
using UsuariosApi.Models;
using UsuariosApi.Repository;
using UsuariosApi.Service;

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

builder.Services
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Registra o handler que sabe avaliar o requirement IdadeMinima.
// Precisa ser registrado como IAuthorizationHandler para o framework encontrá-lo
// automaticamente na hora de avaliar qualquer policy que use esse requirement
builder.Services
    .AddSingleton<IAuthorizationHandler, IdadeAuthorization>();

builder.Services.AddAuthentication
    (options =>
    {
        options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
        new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // ⚠️ PROBLEMA: chave de assinatura hardcoded no código-fonte.
            // Deveria vir de appsettings (fora do Git) ou de um secret manager
            // (dotnet user-secrets em dev, variável de ambiente/Key Vault em prod).
            // Se essa chave vazar, qualquer um pode forjar tokens válidos.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            ("dnbn237g7823rg32gr")),
            // ⚠️ Aceitável para estudo, mas em produção normalmente valida-se
            // Issuer/Audience para impedir reuso de tokens de outro contexto.
            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// Configura o middleware de AUTORIZAÇÃO (o que esse usuário pode fazer?).
// Aqui declaramos a policy "IdadeMinima", exigindo o requirement IdadeMinima(18).
builder.Services.AddAuthorization
    (options =>
    {
        options.AddPolicy("IdadeMinima", policy => 
            policy.AddRequirements(new IdadeMinima(18))

        );
    }
    );


builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<TokenService>();



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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
