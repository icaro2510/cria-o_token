using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
})
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, //Valida o solicitante
        ValidateAudience = true, //Valida quem recebe
        ValidateLifetime = true, //Define se o tempo de expiração será validado
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")),
        ClockSkew = TimeSpan.FromMinutes(30), //Valida o tempo de expiração do token
        ValidIssuer = "exoapi.webapi", //Nome do issuer da origem
        ValidAudience = "exoapi.webapi" //Nome do audience para o destino
    };
});

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
 builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication(); //Habilita a autenticação
app.UseAuthorization(); //Habilita a autorização

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
