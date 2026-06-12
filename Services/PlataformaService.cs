using CineRank.Data;
using CineRank.DTOs;
using CineRank.Models;

namespace CineRank.Services
{
    public class PlataformaService
    {
        private readonly AppDbContext _context;

        public PlataformaService(AppDbContext context)
        {
            _context = context;
        }

        public Plataforma CriarPlataforma(PlataformaCreateDTO plataforma)
        {
            var novaPlataforma = new Plataforma
            {
                NomePlataforma = plataforma.NomePlataforma,
                IconeUrl = plataforma.IconeUrl
            };

            _context.Plataformas.Add(novaPlataforma);
            _context.SaveChanges();

            return novaPlataforma;
        }

        public List<Plataforma> ListarPlataformas()
        {
            return _context.Plataformas.ToList();
        }

        public Plataforma? ObterPlataformaPorId(int id)
        {
            return _context.Plataformas.Find(id);
        }

        public Plataforma? AtualizarPlataforma(int id, PlataformaUpdateDTO plataforma)
        {
            var plataformaExistente = _context.Plataformas.Find(id);
            if (plataformaExistente == null)
            {
                return null;
            }

            if (plataforma.NomePlataforma != null)
            {
                plataformaExistente.NomePlataforma = plataforma.NomePlataforma;
            }

            if (plataforma.IconeUrl != null)
            {
                plataformaExistente.IconeUrl = plataforma.IconeUrl;
            }

            _context.SaveChanges();
            return plataformaExistente;
        }

        public Plataforma? ExcluirPlataforma(int id)
        {
            var plataforma = _context.Plataformas.Find(id);
            if (plataforma == null)
            {
                return null;
            }

            _context.Plataformas.Remove(plataforma);
            _context.SaveChanges();
            return plataforma;
        }
    }
}