using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;

namespace CineRank.Services
{
    public class AuthService
    {
        private readonly AppDbContext _dbContext;
        
        public AuthService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public LoginResponseDTO? Login(string email, string senha)
        {
            var usuario = _dbContext.Usuarios.
            FirstOrDefault(u => u.Email == email);
            if (usuario == null)
            {
                return null;
            }
            if (!BCrypt.Net.BCrypt.Verify(senha, usuario.Senha))
            {
                return null;
            }
            return new LoginResponseDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };

        }
    }
}