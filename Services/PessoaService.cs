using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;

namespace CineRank.Services
{
    public class PessoaService
    {
        private readonly AppDbContext _context;
        public PessoaService(AppDbContext context)
        {
                _context = context;
        }

        public PaginacaoDTO<PessoaSaidaDTO> ListarPessoas(int pagina = 1, int quantidade = 10)
        {
            if (pagina < 1) pagina = 1;
            if (quantidade < 1) quantidade = 10;
            if (quantidade > 50) quantidade = 50;

            var query = _context.Pessoas
                .Select(p => new PessoaSaidaDTO
                {
                    Nome = p.Nome,
                    DataNascimento = p.DataNascimento,
                    Biografia = p.Biografia,
                    FotoUrl = p.FotoUrl
                });

            var total = query.Count();
            var totalPaginas = (int)Math.Ceiling(total / (double)quantidade);

            var dados = query
                .Skip((pagina - 1) * quantidade)
                .Take(quantidade)
                .ToList();

            return new PaginacaoDTO<PessoaSaidaDTO>
            {
                Pagina = pagina,
                Quantidade = quantidade,
                Total = total,
                TotalPaginas = totalPaginas,
                Dados = dados
            };
        }

        public PessoaSaidaDTO? ObterPessoaPorId(int id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa == null) 
                throw new KeyNotFoundException("Pessoa não encontrada");
            return new PessoaSaidaDTO
            {
                Id              = pessoa.Id,
                Nome            = pessoa.Nome,
                Biografia       = pessoa.Biografia,
                DataNascimento  = pessoa.DataNascimento,
                Nacionalidade   = pessoa.Nacionalidade,
                FotoUrl         = pessoa.FotoUrl
            };
        }

        public List<PessoaSaidaDTO> BuscarPessoas(string nome)
        {
            var pessoas = _context.Pessoas
                .Where(p => p.Nome.Contains(nome))
                .Select(p => new PessoaSaidaDTO
                {
                    Id             = p.Id,
                    Nome           = p.Nome,
                    Biografia      = p.Biografia,
                    DataNascimento = p.DataNascimento,
                    Nacionalidade  = p.Nacionalidade,
                    FotoUrl        = p.FotoUrl
                })
                .ToList();

            return pessoas;
        }

        public PessoaSaidaDTO CriarPessoa(PessoaCreateDTO pessoa)
        {
            var novaPessoa = new Pessoa
            {
                Nome            = pessoa.Nome,
                Biografia       = pessoa.Biografia,
                DataNascimento  = pessoa.DataNascimento,
                Nacionalidade   = pessoa.Nacionalidade,
                FotoUrl         = pessoa.FotoUrl
            };

            _context.Pessoas.Add(novaPessoa);
            _context.SaveChanges();

            return new PessoaSaidaDTO
            {
                Id  = novaPessoa.Id,
                Nome            = novaPessoa.Nome,
                Biografia       = novaPessoa.Biografia,
                DataNascimento  = novaPessoa.DataNascimento,
                Nacionalidade   = novaPessoa.Nacionalidade,
                FotoUrl         = novaPessoa.FotoUrl
            };

        }

        public void AtualizarPessoa(int id, PessoaUpdateDTO pessoa)
        {
            var pessoaExistente = _context.Pessoas.Find(id);
            if (pessoaExistente == null) 
            throw new KeyNotFoundException("Pessoa não encontrada");

            if (pessoa.Nome != null)
                pessoaExistente.Nome = pessoa.Nome;

            if (pessoa.Biografia != null)
                pessoaExistente.Biografia = pessoa.Biografia;

            if (pessoa.DataNascimento != null)
                 pessoaExistente.DataNascimento = (DateOnly)pessoa.DataNascimento;

            if (pessoa.Nacionalidade != null)
                pessoaExistente.Nacionalidade = pessoa.Nacionalidade;     
                
            if (pessoa.FotoUrl != null)
                pessoaExistente.FotoUrl = pessoa.FotoUrl;

            _context.SaveChanges();
        }

        public void DeletarPessoa(int id)
        {
            var pessoaExistente = _context.Pessoas.Find(id);
            if (pessoaExistente == null)
                throw new KeyNotFoundException("Pessoa não encontrada");

            _context.Pessoas.Remove(pessoaExistente);
            _context.SaveChanges();
        }
    }
}       