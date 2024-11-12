using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoAPI.Data;
using ProjetoAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configura��o do banco de dados SQLite com ProdutoContext
builder.Services.AddDbContext<ProdutoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ProdutoDB")));

// Configura��o da autentica��o JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,           // Valida o emissor do token
        ValidateAudience = true,         // Valida o p�blico-alvo do token
        ValidateLifetime = true,         // Verifica a expira��o do token
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
        ValidIssuer = "ProdutoAPI",      // Emissor autorizado do token
        ValidAudience = "ProdutoAPIUsers", // P�blico-alvo autorizado
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("Minhas3cret4ChaveParaJWT123456789!")
        ) // Chave de assinatura segura
    };
});

// Adiciona o TokenService para gera��o de tokens JWT
builder.Services.AddSingleton(new TokenService("ProdutoAPI", "ProdutoAPIUsers", "Minhas3cret4ChaveParaJWT123456789!"));

// Configura��o do Swagger com autentica��o JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Produtos",
        Version = "v1",
        Description = "API para gerenciar produtos com opera��es CRUD e autentica��o JWT."
    });

    // Configura a autentica��o no Swagger para aceitar o token JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira 'Bearer' [espa�o] e o token JWT."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Adiciona controladores e endpoints
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Inicializa o banco de dados e aplica migra��es automaticamente ao iniciar a aplica��o
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProdutoContext>();
    context.Database.Migrate();
    context.Seed();
}

// Configura��o de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Middleware de autentica��o
app.UseAuthorization();  // Middleware de autoriza��o

// Mapeia os endpoints dos controladores
app.MapControllers();

app.Run();
