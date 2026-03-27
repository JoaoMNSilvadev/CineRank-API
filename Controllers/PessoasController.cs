using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PessoasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CriarPessoa(PessoaCreateDTO pessoa)
        {
            var novaPessoa = new Pessoa
            {
                Nome = pessoa.Nome,
                Biografia = pessoa.Biografia,
                DataNascimento = pessoa.DataNascimento,
                Nacionalidade = pessoa.Nacionalidade,
                FotoUrl = pessoa.FotoUrl
            };
            _context.Pessoas.Add(novaPessoa);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPessoaPorId),
             new { id = novaPessoa.Id },
              novaPessoa);

        }

        [HttpGet]
        public IActionResult ListarPessoas()
        {
            var pessoas = _context.Pessoas.ToList();
            return Ok(pessoas);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPessoaPorId(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return Ok(pessoa);
        }

        [HttpGet("buscar")]
        public IActionResult BuscarPessoas(string nome)
        {
            var pessoas = _context.Pessoas.Where(p => p.Nome.Contains(nome)).ToList();
            return Ok(pessoas);
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarPessoa(int id, PessoaUpdateDTO pessoa)
        {
            var pessoaExistente = _context.Pessoas.Find(id);
            if (pessoaExistente == null)
            {
                return NotFound();
            }

            if(pessoa.Nome != null)
            {
                pessoaExistente.Nome = pessoa.Nome;
            }
            if(pessoa.Biografia != null)
            {
                pessoaExistente.Biografia = pessoa.Biografia;
            }
            if(pessoa.DataNascimento != null)
            {
                pessoaExistente.DataNascimento = (DateOnly)pessoa.DataNascimento;
            }
            if(pessoa.Nacionalidade != null)
            {
                pessoaExistente.Nacionalidade = pessoa.Nacionalidade;
            }
            if(pessoa.FotoUrl != null)
            {
                pessoaExistente.FotoUrl = pessoa.FotoUrl;
            }

            _context.SaveChanges();
            return Ok(pessoaExistente);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarPessoa(int id)
        {
            var pessoaExistente = _context.Pessoas.Find(id);
            if (pessoaExistente == null)
            {
                return NotFound();
            }

            _context.Pessoas.Remove(pessoaExistente);
            _context.SaveChanges();
            return NoContent();
        }
    }
}