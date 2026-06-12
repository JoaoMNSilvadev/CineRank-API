using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditoController : ControllerBase
    {
        private readonly CreditoService _creditoService;

        public CreditoController(CreditoService creditoService)
        {
            _creditoService = creditoService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AdicionarCredito(int filmeId, int pessoaId, int funcaoId)
        {
            _creditoService.AdicionarCredito(filmeId, pessoaId, funcaoId);
            return Ok("Crédito adicionado com sucesso!");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult RemoverCredito(int id)
        {
            _creditoService.RemoverCredito(id);
            return Ok("Crédito removido.");
        }
    }
}