using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CineRank.Data;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public IActionResult CriarUsuario(UsuarioCreateDTO usuarioDto)
        {
           var novoUsuario = _usuarioService.CriarUsuario(usuarioDto);
            return CreatedAtAction(nameof(BuscarUsuarioPorId), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpGet]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public IActionResult BuscarUsuarioPorId(int id)
        {
            var usuario = _usuarioService.BuscarUsuarioPorId(id);
            return Ok(usuario);
        }

        [HttpPatch("{id}")]
        public IActionResult AtualizarUsuario(int id, UsuarioUpdateDTO usuarioDTO)
        {
            _usuarioService.AtualizarUsuario(id, usuarioDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {
            _usuarioService.DeletarUsuario(id);
            return NoContent();
        }

       [HttpPut("trocar-senha/{id}")]
public IActionResult TrocarSenha(int id, UsuarioTrocarSenhaDTO trocarSenhaDTO)
{
    try 
    {
        _usuarioService.TrocarSenha(id, trocarSenhaDTO);
        return Ok(new { message = "Senha alterada com sucesso!" });
    }
    catch (Exception ex) 
    {
        // Em vez de deixar o sistema travar, retornamos o erro como uma mensagem
        return BadRequest(new { message = ex.Message });
    }
}

    }
}