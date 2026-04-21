using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return CreatedAtAction(nameof(BuscarUsuarioPorId), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ListarUsuarios()
        {
            var usuarios = _usuarioService.ListarUsuarios();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        [Authorize]
        public IActionResult BuscarUsuarioPorId(int id)
        {
                var usuario = _usuarioService.BuscarUsuarioPorId(id);
                return Ok(usuario);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public IActionResult AtualizarUsuario(int id, UsuarioUpdateDTO usuarioDTO)
        {
                _usuarioService.AtualizarUsuario(id, usuarioDTO);
                return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeletarUsuario(int id)
        {
                _usuarioService.DeletarUsuario(id);
                return NoContent();
        }

       [HttpPut("trocar-senha/{id}")]
       [Authorize]
        public IActionResult TrocarSenha(int id, UsuarioTrocarSenhaDTO trocarSenhaDTO)
        {
        _usuarioService.TrocarSenha(id, trocarSenhaDTO);
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