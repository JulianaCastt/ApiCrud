
using System.Linq;
using ProjetoAPI.Models;


namespace ProjetoAPI.Data
{
    public static class ProdutoContextExtensions
    {
        public static void Seed(this ProdutoContext context)
        {
            // Verifica se já existem produtos no banco de dados
            if (!context.Produtos.Any())
            {
                // Adiciona produtos de exemplo
                context.Produtos.AddRange(
                    new Produto { Nome = "Camiseta Básica", Preco = 49.90M, Descricao = "Camiseta de algodão 100%", Estoque = 200 },
                    new Produto { Nome = "Calça Jeans", Preco = 89.90M, Descricao = "Calça jeans masculina", Estoque = 150 },
                    new Produto { Nome = "Jaqueta de Couro", Preco = 199.90M, Descricao = "Jaqueta de couro sintético", Estoque = 50 }
                );

                // Salva as mudanças no banco de dados
                context.SaveChanges();
            }
        }
    }
}
