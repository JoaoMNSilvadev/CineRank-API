using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class FilmeUpdateDTO
    {
        public string? Titulo { get; set; }
        public string? Sinopse { get; set; }
        public string? CapaUrl { get; set; }
        public int? AnoLancamento { get; set; }
        public int? GeneroId { get; set; }
        public int? DiretorId { get; set; }
        public Double? NotaHistoria { get; set; }
        public Double? NotaEmocao { get; set; }
        public Double? NotaDirecao { get; set; }
        public Double? NotaTrilha { get; set; }
        public Double? NotaVisual { get; set; }

        public List<int>? PlataformaIds { get; set; }
        public List<int>? AtorIds { get; set; }
    }
}