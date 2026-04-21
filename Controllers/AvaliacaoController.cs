using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        _avaliacaoService.AdicionarAvaliacao(dto);
        return Ok(new { message = "Avaliação adicionada com sucesso." });
    }
   
     [HttpPatch("{usuarioId}/{filmeId}")]
     [Authorize]
        public IActionResult AtualizarAvaliacao(int usuarioId, int filmeId, AvaliacaoUpdateDTO dto)
        {
           
                _avaliacaoService.AtualizarAvaliacao(usuarioId, filmeId, dto);
                return NoContent();
            }

        [HttpGet("{usuarioId}/{filmeId}")]
        [Authorize]    
        public IActionResult ObterAvaliacao(int usuarioId, int filmeId)
        {
            var avaliacao = _avaliacaoService.ObterMinhaAvaliacao(usuarioId, filmeId);
            if (avaliacao == null)
            {
                return NotFound(new { error = "Avaliação não encontrada para este usuário e filme." });
            }
            return Ok(avaliacao);
        }
    }
}