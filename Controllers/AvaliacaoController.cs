using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacaoController : ControllerBase
    {
        private readonly AvaliacaoService _avaliacaoService;

        public AvaliacaoController(AvaliacaoService avaliacaoService)
        {
            _avaliacaoService = avaliacaoService;
        }

    [HttpPost]
    [Authorize]
    public IActionResult AdicionarAvaliacao(AvaliacaoCreateDTO dto)
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        _avaliacaoService.AdicionarAvaliacao(usuarioId, dto);
        return Ok(new { message = "Avaliação adicionada com sucesso." });
    }
   
     [HttpPatch("{filmeId}")]
     [Authorize]
        public IActionResult AtualizarAvaliacao(int filmeId, AvaliacaoUpdateDTO dto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _avaliacaoService.AtualizarAvaliacao(usuarioId, filmeId, dto);
            return NoContent();
        }

        [HttpGet("{filmeId}")]
        [Authorize]    
        public IActionResult ObterAvaliacao(int filmeId)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var avaliacao = _avaliacaoService.ObterMinhaAvaliacao(usuarioId, filmeId);
            if (avaliacao == null)
            {
                return NotFound(new { error = "Avaliação não encontrada para este usuário e filme." });
            }
            return Ok(avaliacao);
        }
    }
}