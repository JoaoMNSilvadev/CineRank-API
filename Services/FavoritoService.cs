using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;
using Microsoft.EntityFrameworkCore;

namespace CineRank.Services
{
    public class FavoritoService
    {
        private readonly AppDbContext _context;

        public FavoritoService(AppDbContext context)
        {
            _context = context;
        }

        public FavoritoSaidaDTO GetFavorito(int usuarioId, int filmeId)
        {
            var favorito = _context.Favoritos
                .Include(f => f.Filme)
                .FirstOrDefault(f => f.UsuarioId == usuarioId && f.FilmeId == filmeId);

            if (favorito == null)
            {
                throw new KeyNotFoundException("Favorito não encontrado.");
            }

            return new FavoritoSaidaDTO
            {
                UsuarioId = favorito.UsuarioId,
                FilmeId = favorito.FilmeId,
                Titulo = favorito.Filme?.Titulo
            };
        }

        public FavoritoSaidaDTO AddFavorito(int usuarioId, int filmeId)
        {
            var usuarioExiste = _context.Usuarios.Any(u => u.Id == usuarioId);
            if (!usuarioExiste)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            var filme = _context.Filmes.Find(filmeId);
            if (filme == null)
            {
                throw new KeyNotFoundException("Filme não encontrado.");
            }

            var existente = _context.Favoritos.Any(f => f.UsuarioId == usuarioId && f.FilmeId == filmeId);
            if (existente)
            {
                throw new ArgumentException("Este filme já está nos favoritos do usuário.");
            }

            var favorito = new Favorito
            {
                UsuarioId = usuarioId,
                FilmeId = filmeId
            };

            _context.Favoritos.Add(favorito);
            _context.SaveChanges();

            return new FavoritoSaidaDTO
            {
                UsuarioId = favorito.UsuarioId,
                FilmeId = favorito.FilmeId,
                Titulo = filme.Titulo
            };
        }

        public FavoritoSaidaDTO RemoveFavorito(int usuarioId, int filmeId)
        {
            var favorito = _context.Favoritos
                .Include(f => f.Filme)
                .FirstOrDefault(f => f.UsuarioId == usuarioId && f.FilmeId == filmeId);

            if (favorito == null)
            {
                throw new KeyNotFoundException("Favorito não encontrado.");
            }

            _context.Favoritos.Remove(favorito);
            _context.SaveChanges();

            return new FavoritoSaidaDTO
            {
                UsuarioId = favorito.UsuarioId,
                FilmeId = favorito.FilmeId,
                Titulo = favorito.Filme?.Titulo
            };
        }
    }
}