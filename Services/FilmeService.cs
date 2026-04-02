using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.AspNetCore.Mvc;
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
        .Select(f => new FilmeSaidaDTO
        {
            Id = f.Id,
            Titulo = f.Titulo,
            Sinopse = f.Sinopse,
            CapaUrl = f.CapaUrl,
            AnoLancamento = f.AnoLancamento,
            Genero = f.Genero != null ? f.Genero.NomeGenero : "Sem Gênero",
            
            NotaMedia = (f.Avaliacoes != null && f.Avaliacoes.Any()) 
                         ? f.Avaliacoes.Average(a => a.NotaFinal) 
                         : 0,

            // AQUI É ONDE A MÁGICA ACONTECE:
            Creditos = f.Creditos!.Select(c => new FilmeCreditoDTO 
            { 
                PessoaId = c.PessoaId, 
                NomePessoa = c.Pessoa!.Nome, // Puxa o nome da tabela Pessoa
                FuncaoId = c.FuncaoId,
                NomeFuncao = c.Funcao!.Nome  // Puxa o nome da tabela Funcao
            }).ToList(),

            // Se quiser os NOMES das plataformas em vez de IDs:
            PlataformaNomes = f.Plataformas!.Select(p => p.NomePlataforma).ToList()
        }).ToList(); 

    query.ForEach(f => f.NotaMedia = Math.Round(f.NotaMedia, 1));

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
        .Where(f => f.Id == id)
        .Select(f => new FilmeSaidaDTO
        {
            Id = f.Id,
            Titulo = f.Titulo,
            NotaMedia = (f.Avaliacoes != null && f.Avaliacoes.Any()) 
                         ? Math.Round(f.Avaliacoes.Average(a => a.NotaFinal), 1) 
                         : 0,
            // ... demais campos ...
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
                .Where(f => f.Titulo.Contains(titulo))
                .Select(f => new FilmeSaidaDTO
                {
                    Id = f.Id,
                    Titulo = f.Titulo,
                    NotaMedia = (f.Avaliacoes != null && f.Avaliacoes.Any()) 
                                 ? Math.Round(f.Avaliacoes.Average(a => a.NotaFinal), 1) 
                                 : 0,
                    // ... demais campos ...
                })
                .ToList();
        }

public FilmeSaidaDTO CriarFilme(FilmeCreateDTO dto)
{
    var novoFilme = new Filme
    {
        Titulo = dto.Titulo,
        Sinopse = dto.Sinopse,
        CapaUrl = dto.CapaUrl,
        AnoLancamento = dto.AnoLancamento,
        GeneroId = dto.GeneroId,
        Creditos = dto.Creditos?.Select(c => new Credito 
        { 
            PessoaId = c.PessoaId, 
            FuncaoId = c.FuncaoId 
        }).ToList() ?? new List<Credito>()
    };

    // Vincula as entidades de Plataforma ao Filme no banco
    if (dto.PlataformaIds != null && dto.PlataformaIds.Any())
    {
        novoFilme.Plataformas = _context.Plataformas
            .Where(p => dto.PlataformaIds.Contains(p.Id)).ToList();
    }

    _context.Filmes.Add(novoFilme);
    _context.SaveChanges(); 

    // Busca o nome do gênero corretamente usando o GeneroId do filme criado
    var nomeGenero = _context.Generos
        .Where(g => g.Id == novoFilme.GeneroId) 
        .Select(g => g.NomeGenero)
        .FirstOrDefault() ?? "Gênero não encontrado";

    // Busca os nomes das plataformas para o retorno visual
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
        // Mapeia os créditos para o DTO de saída (incluindo os nomes se você ajustou o DTO)
        Creditos = novoFilme.Creditos.Select(c => new FilmeCreditoDTO 
        { 
            PessoaId = c.PessoaId,
            NomePessoa = _context.Pessoas.Find(c.PessoaId)?.Nome ?? "N/A",
            FuncaoId = c.FuncaoId,
            NomeFuncao = _context.Funcoes.Find(c.FuncaoId)?.Nome ?? "N/A"
        }).ToList(),
        PlataformaNomes = nomesDasPlataformas
    };
}
        public void AtualizarFilme(int id, FilmeUpdateDTO filmeDTO)
        {
            var filme = _context.Filmes.Find(id);
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
                var plataformas = _context.Plataformas
                    .Where(p => filmeDTO.PlataformaIds.Contains(p.Id))
                    .ToList();
                filme.Plataformas = plataformas;
            }
            if (filmeDTO.Creditos != null)
            {
                var novosCreditos = filmeDTO.Creditos.Select(c => new Credito
                {
                    PessoaId = c.PessoaId,
                    FuncaoId = c.FuncaoId
                }).ToList();
                filme.Creditos = novosCreditos;
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