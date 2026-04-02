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
        public IActionResult AdicionarOuAtualizarAvaliacao(AvaliacaoCreateDTO dto)
        {
           try
            {
                _avaliacaoService.AdicionarOuAtualizarAvaliacao(dto);
                return Ok(new { message = "Avaliação adicionada ou atualizada com sucesso." });
            }
            catch (Exception ex)
            {
                // Log do erro (ex.Message) pode ser adicionado aqui para diagnóstico
                return BadRequest(new { error = ex.Message });
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