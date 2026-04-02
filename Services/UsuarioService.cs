using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.EntityFrameworkCore;

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
            var novoUsuario = new Usuario
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = usuario.Senha
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
                throw new Exception("Usuário não encontrado.");
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
                throw new Exception("Usuário não encontrado.");
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
                throw new Exception("Usuário não encontrado.");
            }

            _context.Usuarios.Remove(usuarioExistente);
            _context.SaveChanges();
        }


        public void TrocarSenha(int id, UsuarioTrocarSenhaDTO senhaDTO)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                throw new Exception("Usuário não encontrado.");
            }

            if (usuario.Senha != senhaDTO.SenhaAtual)
            {
                throw new Exception("A senha atual está incorreta.");
            }

            usuario.Senha = senhaDTO.NovaSenha;
            _context.SaveChanges();
        }


    }
}