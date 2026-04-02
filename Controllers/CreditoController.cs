using Microsoft.AspNetCore.Mvc;
using CineRank.Models;
using CineRank.Data;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CreditoController : ControllerBase
{
    private readonly AppDbContext _context;

    public CreditoController(AppDbContext context)
    {
        _context = context;
    }

    // Adicionar um crédito (Vincular Pessoa ao Filme com uma Função)
    [HttpPost]
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

    // Remover um crédito
    [HttpDelete("{id}")]
    public IActionResult RemoverCredito(int id)
    {
        var credito = _context.Creditos.Find(id);
        if (credito == null) return NotFound();

        _context.Creditos.Remove(credito);
        _context.SaveChanges();

        return Ok("Crédito removido.");
    }
}