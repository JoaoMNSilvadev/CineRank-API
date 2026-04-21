using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [Authorize(Roles = "Admin")]
        public IActionResult CriarFilme(FilmeCreateDTO filme)
        {
            var novoFilme = _filmeService.CriarFilme(filme);
            return CreatedAtAction(nameof(ObterFilmePorId), new { id = novoFilme.Id }, novoFilme);
        }


        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult ObterFilmePorId(int id)
        {
            var filme = _filmeService.BuscarFilmePorId(id);
            return Ok(filme);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListarFilmes(
            [FromQuery] string ordem = "desc",
             [FromQuery] int pagina = 1,
              [FromQuery] int quantidade = 10)
        {
            var filmes = _filmeService.ListarFilmes(ordem, pagina, quantidade);
            return Ok(filmes);
        }

        [HttpGet("ranking")]
        [AllowAnonymous]
        public IActionResult ObterRankingFilmes(
            [FromQuery] string ordem = "desc",
             [FromQuery] int pagina = 1,
              [FromQuery] int quantidade = 10)
        {
            var filmes = _filmeService.ListarFilmes(ordem, pagina, quantidade);
            return Ok(filmes);
        }

        [HttpGet("buscar")]
        [AllowAnonymous]
        public IActionResult BuscarFilmes(string nome)
        {
            var filmes = _filmeService.BuscarFilmesPorTitulo(nome);
            return Ok(filmes);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AtualizarFilme(int id, FilmeUpdateDTO filme)
        {
                _filmeService.AtualizarFilme(id, filme);
                return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletarFilme(int id)
        {
            _filmeService.DeletarFilme(id);
             return NoContent();
        }
    }
}
