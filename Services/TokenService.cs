using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ProjetoAPI.Services
{
    /// Serviço para geração de tokens JWT, usados na autenticação de usuários.
    public class TokenService
    {
        private readonly string _issuer;      // Identificação da aplicação emissora
        private readonly string _audience;    // Público-alvo do token (os usuários)
        private readonly string _secretKey;   // Chave secreta para assinar o token

        /// Inicializa o serviço de token com emissor, público e chave.
        public TokenService(string issuer, string audience, string secretKey)
        {
            _issuer = issuer;
            _audience = audience;
            _secretKey = secretKey;
        }

        /// Gera um token JWT para o usuário autenticado.
        /// <param name="username">Nome do usuário autenticado</param>
        /// <returns>Token JWT assinado como string</returns>
        public string GenerateToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            // Configura o token com nome do usuário, validade, emissor, público e assinatura
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username) // Define o nome do usuário no token
                }),
                Expires = DateTime.UtcNow.AddHours(1), // Validade do token por 1 hora
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            // Cria e retorna o token em formato de string
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
