using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerosController : ControllerBase
    {
        private readonly GeneroService _generoService;

        public GenerosController(GeneroService generoService)
        {
            _generoService = generoService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CriarGenero(GeneroCreateDTO genero)
        {
            var novoGenero = _generoService.CriarGenero(genero);
            return CreatedAtAction(nameof(ObterGeneroPorId), new { id = novoGenero.Id }, novoGenero);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListarGeneros()
        {
            return Ok(_generoService.ListarGeneros());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult ObterGeneroPorId(int id)
        {
            var genero = _generoService.ObterGeneroPorId(id);
            if (genero == null)
            {
                return NotFound();
            }
            return Ok(genero);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AtualizarGenero(int id, GeneroCreateDTO genero)
        {
            var generoExistente = _generoService.AtualizarGenero(id, genero);
            if (generoExistente == null)
            {
                return NotFound();
            }

            return Ok(generoExistente);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletarGenero(int id)
        {
            var removido = _generoService.DeletarGenero(id);
            if (!removido)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}