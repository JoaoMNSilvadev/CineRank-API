using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.DTOs;
using CineRank.Services;
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
       [HttpPost]
public IActionResult AdicionarAvaliacao(AvaliacaoCreateDTO dto)
{
    try
    {
        _avaliacaoService.AdicionarAvaliacao(dto);
        return Ok(new { message = "Avaliação adicionada com sucesso." });
    }
    catch (Exception ex)
    {
        return BadRequest(new { error = ex.Message });
    }
}

     [HttpPatch("{usuarioId}/{filmeId}")]
        public IActionResult AtualizarAvaliacao(int usuarioId, int filmeId, AvaliacaoUpdateDTO dto)
        {
            try
            {
                _avaliacaoService.AtualizarAvaliacao(usuarioId, filmeId, dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("{usuarioId}/{filmeId}")]
        public IActionResult ObterAvaliacao(int usuarioId, int filmeId)
        {
            var avaliacao = _avaliacaoService.ObterMinhaAvaliacao(usuarioId, filmeId);
            if (avaliacao == null)
            {
                return NotFound();
            }
            return Ok(avaliacao);
        }
    }
}