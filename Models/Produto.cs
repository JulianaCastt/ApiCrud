using System.ComponentModel.DataAnnotations;

namespace ProjetoAPI.Models
{
    
    /// Representa um produto com suas informações de preço, descrição, e controle de estoque.
    public class Produto
    {
        /// Identificador único do produto.
        public int Id { get; set; }


        /// Nome do produto.
        [Required(ErrorMessage = "O nome do produto é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome não pode ter mais de 100 caracteres")]
        public string Nome { get; set; }

        /// Preço do produto.
        [Range(0.01, double.MaxValue, ErrorMessage = "O preço deve ser maior que zero")]
        public decimal Preco { get; set; }

        
        /// Descrição detalhada do produto.
        [StringLength(500, ErrorMessage = "A descrição não pode ter mais de 500 caracteres")]
        public string Descricao { get; set; }

 
        /// Quantidade disponível no estoque.
        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo")]
        public int Estoque { get; set; }

        /// Indica se o produto foi marcado para exclusão lógica.
        public bool IsDeleted { get; set; } = false;
    }
}
