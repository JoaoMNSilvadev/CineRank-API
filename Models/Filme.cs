using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Sinopse { get; set; } = string.Empty;
        public string CapaUrl { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }

        public int GeneroId { get; set; }
        public Genero? Genero { get; set; }

        public ICollection<Avaliacao>? Avaliacoes { get; set; } = new List<Avaliacao>();
        public ICollection<Credito>? Creditos { get; set; }
         public ICollection<Plataforma>? Plataformas { get; set; }
         
    }
}