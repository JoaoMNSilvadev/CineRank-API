using System;
using System.Collections.Generic;
using System.Linq;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.EntityFrameworkCore;

namespace CineRank.Services
{
    public class FilmeService
    {
        private readonly AppDbContext _context;

        public FilmeService(AppDbContext context)
        {
            _context = context;
        }

        public List<FilmeSaidaDTO> ListarFilmes(string ordem = "desc")
        {
            var query = _context.Filmes
                .Include(f => f.Genero)
                .Include(f => f.Avaliacoes)
                .Include(f => f.Creditos!)
                    .ThenInclude(c => c.Pessoa)
                .Include(f => f.Creditos!)
                    .ThenInclude(c => c.Funcao)
                .Include(f => f.Plataformas)
                .Select(f => new FilmeSaidaDTO
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    Sinopse = f.Sinopse,
                    CapaUrl = f.CapaUrl,
                    AnoLancamento = f.AnoLancamento,
                    Genero = f.Genero != null ? f.Genero.NomeGenero : "Sem Gênero",
                    NotaMedia = (f.Avaliacoes != null && f.Avaliacoes.Any())
                                 ? Math.Round(f.Avaliacoes.Average(a => a.NotaFinal), 1)
                                 : 0,
                    Creditos = f.Creditos!.Select(c => new FilmeCreditoDTO
                    {
                        PessoaId = c.PessoaId,
                        NomePessoa = c.Pessoa!.Nome,
                        FuncaoId = c.FuncaoId,
                        NomeFuncao = c.Funcao!.Nome
                    }).ToList(),
                    PlataformaNomes = f.Plataformas!.Select(p => p.NomePlataforma).ToList()
                }).ToList();

            return ordem.ToLower() == "asc"
                   ? query.OrderBy(f => f.NotaMedia).ToList()
                   : query.OrderByDescending(f => f.NotaMedia).ToList();
        }

        public FilmeSaidaDTO? BuscarFilmePorId(int id)
        {
            return _context.Filmes
                .Include(f => f.Genero)
                .Include(f => f.Avaliacoes)
                .Include(f => f.Creditos!)
                    .ThenInclude(c => c.Pessoa)
                .Include(f => f.Creditos!)
                    .ThenInclude(c => c.Funcao)
                .Include(f => f.Plataformas)
                .Where(f => f.Id == id)
                .Select(f => new FilmeSaidaDTO
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    Sinopse = f.Sinopse,
                    CapaUrl = f.CapaUrl,
                    AnoLancamento = f.AnoLancamento,
                    Genero = f.Genero != null ? f.Genero.NomeGenero : "Sem Gênero",
                    NotaMedia = (f.Avaliacoes != null && f.Avaliacoes.Any())
                                 ? Math.Round(f.Avaliacoes.Average(a => a.NotaFinal), 1)
                                 : 0,
                    Creditos = f.Creditos!.Select(c => new FilmeCreditoDTO
                    {
                        PessoaId = c.PessoaId,
                        NomePessoa = c.Pessoa!.Nome,
                        FuncaoId = c.FuncaoId,
                        NomeFuncao = c.Funcao!.Nome
                    }).ToList(),
                    PlataformaNomes = f.Plataformas!.Select(p => p.NomePlataforma).ToList()
                })
                .FirstOrDefault();
        }

        public List<FilmeSaidaDTO> BuscarFilmesPorTitulo(string titulo)
        {
            return _context.Filmes
                .Include(f => f.Genero)
                .Include(f => f.Avaliacoes)
                .Include(f => f.Creditos!)
                    .ThenInclude(c => c.Pessoa)
                .Include(f => f.Creditos!)
                    .ThenInclude(c => c.Funcao)
                .Include(f => f.Plataformas)
                .Where(f => f.Titulo.Contains(titulo))
                .Select(f => new FilmeSaidaDTO
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    Sinopse = f.Sinopse,
                    CapaUrl = f.CapaUrl,
                    AnoLancamento = f.AnoLancamento,
                    Genero = f.Genero != null ? f.Genero.NomeGenero : "Sem Gênero",
                    NotaMedia = (f.Avaliacoes != null && f.Avaliacoes.Any())
                                 ? Math.Round(f.Avaliacoes.Average(a => a.NotaFinal), 1)
                                 : 0,
                    Creditos = f.Creditos!.Select(c => new FilmeCreditoDTO
                    {
                        PessoaId = c.PessoaId,
                        NomePessoa = c.Pessoa!.Nome,
                        FuncaoId = c.FuncaoId,
                        NomeFuncao = c.Funcao!.Nome
                    }).ToList(),
                    PlataformaNomes = f.Plataformas!.Select(p => p.NomePlataforma).ToList()
                })
                .ToList();
        }

        public FilmeSaidaDTO CriarFilme(FilmeCreateDTO dto)
        {
            // Carrega pessoas e funções de uma vez — evita N+1 queries
            var pessoaIds = dto.Creditos.Select(c => c.PessoaId).ToList();
            var funcaoIds = dto.Creditos.Select(c => c.FuncaoId).ToList();

            var pessoas = _context.Pessoas
                .Where(p => pessoaIds.Contains(p.Id))
                .ToDictionary(p => p.Id, p => p.Nome);

            var funcoes = _context.Funcoes
                .Where(f => funcaoIds.Contains(f.Id))
                .ToDictionary(f => f.Id, f => f.Nome);

            var novoFilme = new Filme
            {
                Titulo = dto.Titulo,
                Sinopse = dto.Sinopse,
                CapaUrl = dto.CapaUrl,
                AnoLancamento = dto.AnoLancamento,
                GeneroId = dto.GeneroId,
                Creditos = dto.Creditos.Select(c => new Credito
                {
                    PessoaId = c.PessoaId,
                    FuncaoId = c.FuncaoId
                }).ToList()
            };

            if (dto.PlataformaIds != null && dto.PlataformaIds.Any())
            {
                novoFilme.Plataformas = _context.Plataformas
                    .Where(p => dto.PlataformaIds.Contains(p.Id)).ToList();
            }

            _context.Filmes.Add(novoFilme);
            _context.SaveChanges();

            var nomeGenero = _context.Generos
                .Where(g => g.Id == novoFilme.GeneroId)
                .Select(g => g.NomeGenero)
                .FirstOrDefault() ?? "Gênero não encontrado";

            var nomesDasPlataformas = novoFilme.Plataformas?
                .Select(p => p.NomePlataforma)
                .ToList() ?? new List<string>();

            return new FilmeSaidaDTO
            {
                Id = novoFilme.Id,
                Titulo = novoFilme.Titulo,
                Sinopse = novoFilme.Sinopse,
                CapaUrl = novoFilme.CapaUrl,
                AnoLancamento = novoFilme.AnoLancamento,
                Genero = nomeGenero,
                NotaMedia = 0,
                Creditos = novoFilme.Creditos.Select(c => new FilmeCreditoDTO
                {
                    PessoaId = c.PessoaId,
                    NomePessoa = pessoas.GetValueOrDefault(c.PessoaId, "N/A"),
                    FuncaoId = c.FuncaoId,
                    NomeFuncao = funcoes.GetValueOrDefault(c.FuncaoId, "N/A")
                }).ToList(),
                PlataformaNomes = nomesDasPlataformas
            };
        }

        public void AtualizarFilme(int id, FilmeUpdateDTO filmeDTO)
        {
            var filme = _context.Filmes
                .Include(f => f.Plataformas)
                .Include(f => f.Creditos)
                .FirstOrDefault(f => f.Id == id);

            if (filme == null)
                throw new Exception("Filme não encontrado.");

            if (filmeDTO.Titulo != null)
                filme.Titulo = filmeDTO.Titulo;
            if (filmeDTO.Sinopse != null)
                filme.Sinopse = filmeDTO.Sinopse;
            if (filmeDTO.CapaUrl != null)
                filme.CapaUrl = filmeDTO.CapaUrl;
            if (filmeDTO.AnoLancamento != null)
                filme.AnoLancamento = filmeDTO.AnoLancamento.Value;
            if (filmeDTO.GeneroId != null)
                filme.GeneroId = filmeDTO.GeneroId.Value;
            if (filmeDTO.PlataformaIds != null)
            {
                filme.Plataformas = _context.Plataformas
                    .Where(p => filmeDTO.PlataformaIds.Contains(p.Id))
                    .ToList();
            }
            if (filmeDTO.Creditos != null)
            {
                filme.Creditos = filmeDTO.Creditos.Select(c => new Credito
                {
                    PessoaId = c.PessoaId,
                    FuncaoId = c.FuncaoId
                }).ToList();
            }

            _context.SaveChanges();
        }

        public void DeletarFilme(int id)
        {
            var filme = _context.Filmes.Find(id);
            if (filme == null)
                throw new Exception("Filme não encontrado.");

            _context.Filmes.Remove(filme);
            _context.SaveChanges();
        }
    }
}
