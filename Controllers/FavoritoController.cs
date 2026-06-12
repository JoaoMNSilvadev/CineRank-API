using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritoController : ControllerBase
    {
        private readonly FavoritoService _favoritoService;

        public FavoritoController(FavoritoService favoritoService)
        {
            _favoritoService = favoritoService;
        }

        [HttpGet("{filmeId}")]
        [Authorize]
        public IActionResult GetFavorito(int filmeId)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            return Ok(_favoritoService.GetFavorito(usuarioId, filmeId));
        }

        [HttpPost("{filmeId}")]
        [Authorize]
        public IActionResult AddFavorito(int filmeId)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var favorito = _favoritoService.AddFavorito(usuarioId, filmeId);
            return CreatedAtAction(nameof(GetFavorito), new { filmeId = favorito.FilmeId }, favorito);
        }

        [HttpDelete("{filmeId}")]
        [Authorize]
        public IActionResult RemoveFavorito(int filmeId)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _favoritoService.RemoveFavorito(usuarioId, filmeId);
            return NoContent();
        }
    }
}