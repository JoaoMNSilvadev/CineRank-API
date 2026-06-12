using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;

namespace CineRank.Services
{
    public class GeneroService
    {
        private readonly AppDbContext _context;

        public GeneroService(AppDbContext context)
        {
            _context = context;
        }

        public Genero CriarGenero(GeneroCreateDTO genero)
        {
            var novoGenero = new Genero
            {
                NomeGenero = genero.NomeGenero
            };

            _context.Generos.Add(novoGenero);
            _context.SaveChanges();

            return novoGenero;
        }

        public List<Genero> ListarGeneros()
        {
            return _context.Generos.ToList();
        }

        public Genero? ObterGeneroPorId(int id)
        {
            return _context.Generos.Find(id);
        }

        public Genero? AtualizarGenero(int id, GeneroCreateDTO genero)
        {
            var generoExistente = _context.Generos.Find(id);
            if (generoExistente == null)
            {
                return null;
            }

            generoExistente.NomeGenero = genero.NomeGenero;
            _context.SaveChanges();

            return generoExistente;
        }

        public bool DeletarGenero(int id)
        {
            var generoExistente = _context.Generos.Find(id);
            if (generoExistente == null)
            {
                return false;
            }

            _context.Generos.Remove(generoExistente);
            _context.SaveChanges();
            return true;
        }
    }
}