using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjetoAPI.Data;
using ProjetoAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração do banco de dados SQLite com ProdutoContext
builder.Services.AddDbContext<ProdutoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ProdutoDB")));

// Configuração da autenticação JWT
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
        ValidateAudience = true,         // Valida o público-alvo do token
        ValidateLifetime = true,         // Verifica a expiração do token
        ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
        ValidIssuer = "ProdutoAPI",      // Emissor autorizado do token
        ValidAudience = "ProdutoAPIUsers", // Público-alvo autorizado
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("Minhas3cret4ChaveParaJWT123456789!")
        ) // Chave de assinatura segura
    };
});

// Adiciona o TokenService para geração de tokens JWT
builder.Services.AddSingleton(new TokenService("ProdutoAPI", "ProdutoAPIUsers", "Minhas3cret4ChaveParaJWT123456789!"));

// Configuração do Swagger com autenticação JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "API de Produtos",
        Version = "v1",
        Description = "API para gerenciar produtos com operações CRUD e autenticação JWT."
    });

    // Configura a autenticação no Swagger para aceitar o token JWT
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira 'Bearer' [espaço] e o token JWT."
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

// Inicializa o banco de dados e aplica migrações automaticamente ao iniciar a aplicação
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ProdutoContext>();
    context.Database.Migrate();
    context.Seed();
}

// Configuração de middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Middleware de autenticação
app.UseAuthorization();  // Middleware de autorização

// Mapeia os endpoints dos controladores
app.MapControllers();

app.Run();
