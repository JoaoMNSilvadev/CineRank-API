using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FuncaoController : ControllerBase
    {
        private readonly FuncaoService _funcaoService;

        public FuncaoController(FuncaoService funcaoService)
        {
            _funcaoService = funcaoService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Listar()
        {
            return Ok(_funcaoService.Listar());
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Criar(FuncaoCreateDTO dto)
        {
            var novaFuncao = _funcaoService.Criar(dto);
            return Ok(novaFuncao);
        }

        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Deletar(int id)
        {
            var funcao = _funcaoService.Deletar(id);
            if (funcao == null) return NotFound();

            return Ok("Função removida com sucesso.");
        }
    }
}