using CineRank.DTOs;
using CineRank.Services;
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
        _usuarioService.TrocarSenha(id, trocarSenhaDTO);
        return Ok(new { message = "Senha alterada com sucesso!" });
}

    }
}