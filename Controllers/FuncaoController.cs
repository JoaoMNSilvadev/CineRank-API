using Microsoft.AspNetCore.Mvc;
using CineRank.Data;
using CineRank.Models;
using System.Linq;

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

        // Listar todas as funções (Ator, Diretor, etc)
        [HttpGet]
        public IActionResult Listar()
        {
            var funcoes = _context.Funcoes.ToList();
            return Ok(funcoes);
        }

        // Cadastrar uma nova função
        [HttpPost]
        public IActionResult Criar(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest("O nome da função é obrigatório.");

            var novaFuncao = new Funcao { Nome = nome };
            _context.Funcoes.Add(novaFuncao);
            _context.SaveChanges();

            return Ok(novaFuncao);
        }

        // Deletar uma função
        [HttpDelete("{id}")]
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