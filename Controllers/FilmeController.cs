using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilmeController : ControllerBase
    {
        private readonly FilmeService _filmeService;

        public FilmeController(FilmeService filmeService)
        {
            _filmeService = filmeService;
        }
        

[HttpPost]
public IActionResult CriarFilme(FilmeCreateDTO filme)
{
    var novoFilme = _filmeService.CriarFilme(filme);
    return CreatedAtAction(nameof(ObterFilmePorId),
     new { id = novoFilme.Id },
      novoFilme);
}
      



[HttpGet("{id}")]
public IActionResult ObterFilmePorId(int id)
        {
            var filme = _filmeService.BuscarFilmePorId(id);
              
            if (filme == null)
            {
                return NotFound();
            }
            return Ok(filme);
        }

    [HttpGet]
    public IActionResult ListarFilmes()
    {
        var filmes = _filmeService.ListarFilmes();
           
        return Ok(filmes);
    }

   [HttpGet("ranking")]
    public IActionResult ObterRankingFilmes([FromQuery] string ordem = "desc")
    {
        var filmes = _filmeService.ListarFilmes(ordem);
        return Ok(filmes);
    }

    [HttpGet("buscar")]
    public IActionResult BuscarFilmes(string nome)
    {
        var filmes = _filmeService.BuscarFilmesPorTitulo(nome);
        return Ok(filmes);
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizarFilme(int id, FilmeUpdateDTO filme)
    {
        try
        {
            _filmeService.AtualizarFilme(id, filme);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }    
        return NoContent();
}


    [HttpDelete("{id}")]
    public IActionResult DeletarFilme(int id)
    {
     try
        {
            _filmeService.DeletarFilme(id);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }    
        return NoContent();
    }
 }
}