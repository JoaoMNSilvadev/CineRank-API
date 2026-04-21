using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly PessoaService _pessoaService;

        public PessoasController(PessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CriarPessoa(PessoaCreateDTO pessoa)
        {
            var novaPessoa = _pessoaService.CriarPessoa(pessoa);
            return CreatedAtAction(nameof(ObterPessoaPorId), new { id = novaPessoa.Id }, novaPessoa);

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListarPessoas([FromQuery] int pagina = 1, [FromQuery] int quantidade = 10)
        {
            var pessoas = _pessoaService.ListarPessoas(pagina, quantidade);
            return Ok(pessoas);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult ObterPessoaPorId(int id)
        {
            var pessoa = _pessoaService.ObterPessoaPorId(id);
            return Ok(pessoa);
        }

        [HttpGet("buscar")]
        [AllowAnonymous]
        public IActionResult BuscarPessoas(string nome)
        {
            var pessoas = _pessoaService.BuscarPessoas(nome);
            return Ok(pessoas);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AtualizarPessoa(int id, PessoaUpdateDTO pessoa)
        {
            _pessoaService.AtualizarPessoa(id, pessoa);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletarPessoa(int id)
        {
            _pessoaService.DeletarPessoa(id);
            return NoContent();
        }
    }
}