using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoAPI.Data;
using ProjetoAPI.Models;
using ProjetoAPI.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ProdutoContext _context;
        private readonly TokenService _tokenService;

        public ProdutosController(ProdutoContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // Retorna todos os produtos disponíveis (não excluídos)
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Produto>), 200)]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            var produtos = await _context.Produtos
                                         .Where(p => !p.IsDeleted)
                                         .ToListAsync();

            return Ok(produtos);
        }

        // Autentica o usuário e gera um token JWT
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(400)]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest?.Username))
            {
                return BadRequest("O campo 'username' é obrigatório.");
            }

            var token = _tokenService.GenerateToken(loginRequest.Username);

            return Ok(new { Token = token });
        }

        // Retorna um produto específico pelo ID
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(Produto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var produto = await _context.Produtos
                                        .Where(p => !p.IsDeleted && p.Id == id)
                                        .FirstOrDefaultAsync();

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        // Adiciona um novo produto
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(Produto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Produto>> PostProduto([FromBody] Produto produto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduto), new { id = produto.Id }, produto);
        }

        // Atualiza os dados de um produto específico
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutProduto(int id, [FromBody] Produto produto)
        {
            if (id != produto.Id)
            {
                return BadRequest();
            }

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Marca um produto como excluído logicamente
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound();
            }

            produto.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return _context.Produtos.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}
