using Microsoft.AspNetCore.Mvc;
using CineRank.Data;
using CineRank.Models;
using Microsoft.AspNetCore.Authorization;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncaoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Listar()
        {
            var funcoes = _context.Funcoes.ToList();
            return Ok(funcoes);
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Criar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("O nome da função é obrigatório.");

            var novaFuncao = new Funcao { Nome = nome };
            _context.Funcoes.Add(novaFuncao);
            _context.SaveChanges();

            return Ok(novaFuncao);
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Deletar(int id)
        {
            var funcao = _context.Funcoes.Find(id);
            if (funcao == null) return NotFound();

            _context.Funcoes.Remove(funcao);
            _context.SaveChanges();

            return Ok("Função removida com sucesso.");
        }
    }
}