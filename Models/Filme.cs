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

        public int DiretorId { get; set; }
        public Pessoa? Diretor { get; set; }

        public Double NotaHistoria { get; set; }
        public Double NotaEmocao { get; set; }
        public Double NotaDirecao { get; set; }
        public Double NotaTrilha { get; set; }
        public Double NotaVisual { get; set; }

        public Double NotaFinal { get{ return (NotaHistoria * 4 + NotaEmocao * 3 + NotaDirecao * 2 + NotaTrilha * 1 + NotaVisual * 1) / 11; } }
   
        public ICollection<Plataforma>? Plataformas { get; set; }
        public ICollection<Pessoa>? Atores { get; set; }
    }
}