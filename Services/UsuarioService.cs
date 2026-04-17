using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;

namespace CineRank.Services
{
    public class UsuarioService
    {
        private readonly AppDbContext _context;
        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public UsuarioSaidaDTO CriarUsuario(UsuarioCreateDTO usuario)
        {   
            var emailExistente = _context.Usuarios.Any(u => u.Email == usuario.Email);
            if (emailExistente) {
                throw new ArgumentException("O email já está em uso por outro usuário.");
            }

            var novoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha)
            };
            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();
            return new UsuarioSaidaDTO
            {
                Id = novoUsuario.Id,
                Nome = novoUsuario.Nome,
                Email = novoUsuario.Email
            };

        }

        public void AtualizarUsuario(int id, UsuarioUpdateDTO usuarioDTO)
        {
            var usuarioExistente = _context.Usuarios.Find(id);

            if (usuarioExistente == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }
            if (usuarioDTO.Nome != null)
                usuarioExistente.Nome = usuarioDTO.Nome;
            if (usuarioDTO.Email != null)
                usuarioExistente.Email = usuarioDTO.Email;

            _context.SaveChanges();

        }

        public List<UsuarioSaidaDTO> ListarUsuarios()
        {
            return _context.Usuarios
                .Select(u => new UsuarioSaidaDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    Email = u.Email
                })
                .ToList();
        }

        public UsuarioSaidaDTO BuscarUsuarioPorId(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }
            return new UsuarioSaidaDTO
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email
            };
        }

        public void DeletarUsuario(int id)
        {
            var usuarioExistente = _context.Usuarios.Find(id);

            if (usuarioExistente == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            _context.Usuarios.Remove(usuarioExistente);
            _context.SaveChanges();
        }


        public void TrocarSenha(int id, UsuarioTrocarSenhaDTO senhaDTO)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            if (!BCrypt.Net.BCrypt.Verify(senhaDTO.SenhaAtual, usuario.Senha))
            {
                throw new ArgumentException("A senha atual está incorreta.");
            }

            usuario.Senha = BCrypt.Net.BCrypt.HashPassword(senhaDTO.NovaSenha);
            _context.SaveChanges();
        }


    }
}