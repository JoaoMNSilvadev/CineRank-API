using CineRank.Data;
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

        public Usuario? Login(string email, string senha)
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
            return usuario;

        }
    }
}