using Microsoft.EntityFrameworkCore;
using ProjetoAPI.Models;

namespace ProjetoAPI.Data
{
    /// Contexto do banco de dados para gerenciar os dados de Produto
    public class ProdutoContext : DbContext
    {
        /// Inicializa o contexto com as configurações especificadas
        public ProdutoContext(DbContextOptions<ProdutoContext> options) : base(options)
        {
        }

        /// Representa a tabela de Produtos no banco de dados
        public DbSet<Produto> Produtos { get; set; } = default!;

        /// Configurações adicionais do modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Adiciona um índice para o campo Nome, permitindo valores repetidos
            modelBuilder.Entity<Produto>()
                .HasIndex(p => p.Nome)
                .IsUnique(false);
        }
    }
}
