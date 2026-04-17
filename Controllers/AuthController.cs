using CineRank.DTOs;
using CineRank.Services;
using Microsoft.AspNetCore.Mvc;

namespace CineRank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO loginDTO)
        {
            var resultado = _authService.Login(loginDTO.Email, loginDTO.Senha);
            if (resultado == null)
            {
                return Unauthorized(new { message = "Email ou senha inválidos." });
            }
            return Ok(new {
                 message = "Login realizado com sucesso.", usuario = resultado
                 });

        }
    }
}