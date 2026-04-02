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
        public List<int>? PlataformaIds { get; set; }
        public List<FilmeCreditoDTO>? Creditos { get; set; }

    }
}