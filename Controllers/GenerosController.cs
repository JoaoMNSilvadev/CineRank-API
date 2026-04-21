using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenerosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GenerosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CriarGenero(GeneroCreateDTO genero)
        {
            var novoGenero = new Genero
            {
                NomeGenero = genero.NomeGenero
            };
            _context.Generos.Add(novoGenero);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterGeneroPorId),
            new{ id = novoGenero.Id},
             novoGenero); 

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListarGeneros()
        {
            var generos = _context.Generos.ToList();
            return Ok(generos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult ObterGeneroPorId(int id)
        {
            var genero = _context.Generos.Find(id);
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
            var generoExistente = _context.Generos.Find(id);
            if (generoExistente == null)
            {
                return NotFound();
            }

            generoExistente.NomeGenero = genero.NomeGenero;
            _context.SaveChanges();
            return Ok(generoExistente);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletarGenero(int id)
        {
            var generoExistente = _context.Generos.Find(id);
            if (generoExistente == null)
            {
                return NotFound();
            }

            _context.Generos.Remove(generoExistente);
            _context.SaveChanges();
            return NoContent();
        }


    }
}