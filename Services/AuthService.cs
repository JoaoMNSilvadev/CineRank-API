using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.IdentityModel.Tokens;

namespace CineRank.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        
        public AuthService(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public string Login(string email, string senha)
        {
            var usuario = _dbContext.Usuarios.
            FirstOrDefault(u => u.Email == email);
            if (usuario == null)
            {
                throw new ArgumentException("Email ou senha inválidos.");
            }
            if (!BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
            {
                throw new ArgumentException("Email ou senha inválidos.");
            }
            return GerarToken(usuario);

        }

        private string GerarToken(Usuario usuario)
        {
           var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Role)
            };

            var chave = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
                var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(8),
                claims: claims,
                signingCredentials: credenciais);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}