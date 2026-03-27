using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmeController(AppDbContext context)
        {
            _context = context;
        }

[HttpPost]
public IActionResult CriarFilme(FilmeCreateDTO filme)
{
    var novoFilme = new Models.Filme
    {
        Titulo = filme.Titulo,
        Sinopse = filme.Sinopse,
        CapaUrl = filme.CapaUrl,
        AnoLancamento = filme.AnoLancamento,
        GeneroId = filme.GeneroId,
        DiretorId = filme.DiretorId,
        NotaHistoria = filme.NotaHistoria,
        NotaEmocao = filme.NotaEmocao,
        NotaDirecao = filme.NotaDirecao,
        NotaTrilha = filme.NotaTrilha,
        NotaVisual = filme.NotaVisual
    };
    if (filme.AtorIds != null && filme.AtorIds.Any())
    {
        novoFilme.Atores = _context.Pessoas
            .Where(p => filme.AtorIds.Contains(p.Id)).ToList();
    }

    if (filme.PlataformaIds != null && filme.PlataformaIds.Any())
    {
        novoFilme.Plataformas = _context.Plataformas
            .Where(p => filme.PlataformaIds.Contains(p.Id)).ToList();
    }
    _context.Filmes.Add(novoFilme);
    _context.SaveChanges();
    return CreatedAtAction(nameof(ObterFilmePorId),
     new { id = novoFilme.Id },
      novoFilme);
}


[HttpGet("{id}")]
public IActionResult ObterFilmePorId(int id)
        {
            var filme = _context.Filmes
                .Include(f => f.Genero)
                .Include(f => f.Diretor)
                .Include(f => f.Atores)
                .Include(f => f.Plataformas)
                .FirstOrDefault(f => f.Id == id);
            if (filme == null)
            {
                return NotFound();
            }
            return Ok(filme);
        }

    [HttpGet]
    public IActionResult ListarFilmes()
    {
        var filmes = _context.Filmes
            .Include(f => f.Genero)
            .Include(f => f.Diretor)
            .Include(f => f.Atores)
            .Include(f => f.Plataformas)
            .ToList();
        return Ok(filmes);
    }

    [HttpGet("Ranking")]
    public IActionResult ObterRankingFilmes()
    {
        var filmes = _context.Filmes
            .Include(f => f.Genero)
            .Include(f => f.Diretor)
            .Include(f => f.Atores)
            .Include(f => f.Plataformas)
            .OrderByDescending(f => f.NotaFinal)
            .ToList();
        return Ok(filmes);
    }

    [HttpGet("buscar")]
    public IActionResult BuscarFilmes(string nome)
    {
        var filmes = _context.Filmes
            .Include(f => f.Genero)
            .Include(f => f.Diretor)
            .Include(f => f.Atores)
            .Include(f => f.Plataformas)
            .Where(f => f.Titulo.Contains(nome))
            .ToList();
        return Ok(filmes);
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizarFilme(int id, FilmeUpdateDTO filme)
    {
        var filmeExistente = _context.Filmes
            .Include(f => f.Atores)
            .Include(f => f.Plataformas)
            .FirstOrDefault(f => f.Id == id);
        if (filmeExistente == null)
        {
            return NotFound();
        }

        if (filme.Titulo != null)
        {
            filmeExistente.Titulo = filme.Titulo;
        }
        if (filme.Sinopse != null)
        {
            filmeExistente.Sinopse = filme.Sinopse;
        }
        if (filme.CapaUrl != null)
        {
            filmeExistente.CapaUrl = filme.CapaUrl;
        }
        if (filme.AnoLancamento != null)
        {
            filmeExistente.AnoLancamento = (int)filme.AnoLancamento;
        }
        if (filme.GeneroId != null)
        {
            filmeExistente.GeneroId = (int)filme.GeneroId;
        }
        if (filme.DiretorId != null)
        {
            filmeExistente.DiretorId = (int)filme.DiretorId;
        }
        if (filme.NotaHistoria != null)
        {
            filmeExistente.NotaHistoria = (double)filme.NotaHistoria;
        }
        if (filme.NotaEmocao != null)
        {
            filmeExistente.NotaEmocao = (double)filme.NotaEmocao;
        }
        if (filme.NotaDirecao != null)
        {
            filmeExistente.NotaDirecao = (double)filme.NotaDirecao;
        }
        if (filme.NotaTrilha != null)
        {
            filmeExistente.NotaTrilha = (double)filme.NotaTrilha;
        }
        if (filme.NotaVisual != null)
        {
            filmeExistente.NotaVisual = (double)filme.NotaVisual;
        }

         if (filme.AtorIds != null && filme.AtorIds.Any())
    {
         var atores = _context.Pessoas
             .Where(p => filme.AtorIds.Contains(p.Id)).ToList();
         filmeExistente.Atores = atores;
     }

     if (filme.PlataformaIds != null && filme.PlataformaIds.Any())
     {
         var plataformas = _context.Plataformas
             .Where(p => filme.PlataformaIds.Contains(p.Id)).ToList();
         filmeExistente.Plataformas = plataformas;
      }

        _context.SaveChanges();
        return NoContent();
}


    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
        var filmeExistente = _context.Filmes.Find(id);
        if (filmeExistente == null)
        {
            return NotFound();
        }

        _context.Filmes.Remove(filmeExistente);
        _context.SaveChanges();
        return NoContent();
    }
 }
}