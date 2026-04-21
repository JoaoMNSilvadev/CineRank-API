using Microsoft.AspNetCore.Mvc;
using CineRank.Models;
using CineRank.Data;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class CreditoController : ControllerBase
{
    private readonly AppDbContext _context;

    public CreditoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public IActionResult AdicionarCredito(int filmeId, int pessoaId, int funcaoId)
    {
        // Verifica se a combinação já existe para não duplicar
        var existe = _context.Creditos.Any(c => 
            c.FilmeId == filmeId && c.PessoaId == pessoaId && c.FuncaoId == funcaoId);

        if (existe) return BadRequest("Este crédito já foi cadastrado.");

        var credito = new Credito 
        { 
            FilmeId = filmeId, 
            PessoaId = pessoaId, 
            FuncaoId = funcaoId 
        };

        _context.Creditos.Add(credito);
        _context.SaveChanges();

        return Ok("Crédito adicionado com sucesso!");
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult RemoverCredito(int id)
    {
        var credito = _context.Creditos.Find(id);
        if (credito == null) return NotFound();

        _context.Creditos.Remove(credito);
        _context.SaveChanges();

        return Ok("Crédito removido.");
    }
}