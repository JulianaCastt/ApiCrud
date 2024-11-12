using System.ComponentModel.DataAnnotations;

namespace ProjetoAPI.Models
{
    
    /// Modelo de requisição para autenticação de usuário.
    public class LoginRequest
    {
       
        /// Nome de usuário para autenticação.
        [Required(ErrorMessage = "O campo 'username' é obrigatório.")]
        public string Username { get; set; }
    }
}
