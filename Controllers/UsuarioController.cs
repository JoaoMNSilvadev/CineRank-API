using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [AllowAnonymous]
        public IActionResult CriarUsuario(UsuarioCreateDTO usuarioDto)
        {
           var novoUsuario = _usuarioService.CriarUsuario(usuarioDto);
            return CreatedAtAction(nameof(BuscarMeuUsuario), new { }, novoUsuario);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult BuscarMeuUsuario()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var usuario = _usuarioService.BuscarUsuarioPorId(usuarioId);
            return Ok(usuario);
        }

        [HttpPatch("me")]
        [Authorize]
        public IActionResult AtualizarUsuario(UsuarioUpdateDTO usuarioDTO)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _usuarioService.AtualizarUsuario(usuarioId, usuarioDTO);
            return NoContent();

        }

        [HttpDelete("me")]
        [Authorize]
        public IActionResult DeletarUsuario()
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _usuarioService.DeletarUsuario(usuarioId);
            return NoContent();
        }

       [HttpPut("trocar-senha/me")]
       [Authorize]
        public IActionResult TrocarSenha(UsuarioTrocarSenhaDTO trocarSenhaDTO)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            _usuarioService.TrocarSenha(usuarioId, trocarSenhaDTO);
            return Ok(new { message = "Senha alterada com sucesso!" });
        }

        [HttpPatch("{id}/role")]
        [Authorize (Roles = "Admin")]
        public IActionResult AlterarRole(int id, [FromBody] string novaRole)
        {
            _usuarioService.AlterarRole(id, novaRole);
            return Ok(new { message = "Role alterada com sucesso!" });
        }

    }
}