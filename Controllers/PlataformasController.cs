using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlataformasController : ControllerBase
    {
     private readonly AppDbContext _context;

        public PlataformasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CriarPlataforma(PlataformaCreateDTO plataforma)
        {
            var novaPlataforma = new Plataforma
            {
                NomePlataforma = plataforma.NomePlataforma,
                IconeUrl = plataforma.IconeUrl
            };
            _context.Plataformas.Add(novaPlataforma);
            _context.SaveChanges();
            return CreatedAtAction(nameof(ObterPlataformaPorId),
             new { id = novaPlataforma.Id },
              novaPlataforma);
        }

        [HttpGet]
        public IActionResult ListarPlataformas()
        {
            var plataformas = _context.Plataformas.ToList();
            return Ok(plataformas);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPlataformaPorId(int id)
        {
            var plataforma = _context.Plataformas.Find(id);
            if (plataforma == null)
            {
                return NotFound();
            }
            return Ok(plataforma);
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarPlataforma(int id, PlataformaUpdateDTO plataforma)
        {
            var plataformaExistente = _context.Plataformas.Find(id);
            if (plataformaExistente == null)
            {
                return NotFound();
            }

            if (plataforma.NomePlataforma != null)
            {
                plataformaExistente.NomePlataforma = plataforma.NomePlataforma;
            }
            if (plataforma.IconeUrl != null)
            {
                plataformaExistente.IconeUrl = plataforma.IconeUrl;
            }

            _context.SaveChanges();
            return Ok(plataformaExistente);
        }

        [HttpDelete("{id}")]
        public IActionResult ExcluirPlataforma(int id)
        {
            var plataforma = _context.Plataformas.Find(id);
            if (plataforma == null)
            {
                return NotFound();
            }

            _context.Plataformas.Remove(plataforma);
            _context.SaveChanges();
            return Ok(plataforma);
        }

    }
}