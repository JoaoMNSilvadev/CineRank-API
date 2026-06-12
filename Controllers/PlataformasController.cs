using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlataformasController : ControllerBase
    {
        private readonly PlataformaService _plataformaService;

        public PlataformasController(PlataformaService plataformaService)
        {
            _plataformaService = plataformaService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult CriarPlataforma(PlataformaCreateDTO plataforma)
        {
            var novaPlataforma = _plataformaService.CriarPlataforma(plataforma);
            return CreatedAtAction(nameof(ObterPlataformaPorId), new { id = novaPlataforma.Id }, novaPlataforma);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ListarPlataformas()
        {
            return Ok(_plataformaService.ListarPlataformas());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult ObterPlataformaPorId(int id)
        {
            var plataforma = _plataformaService.ObterPlataformaPorId(id);
            if (plataforma == null)
            {
                return NotFound();
            }
            return Ok(plataforma);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult AtualizarPlataforma(int id, PlataformaUpdateDTO plataforma)
        {
            var plataformaExistente = _plataformaService.AtualizarPlataforma(id, plataforma);
            if (plataformaExistente == null)
            {
                return NotFound();
            }
            return Ok(plataformaExistente);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult ExcluirPlataforma(int id)
        {
            var plataforma = _plataformaService.ExcluirPlataforma(id);
            if (plataforma == null)
            {
                return NotFound();
            }

            return Ok(plataforma);
        }

    }
}