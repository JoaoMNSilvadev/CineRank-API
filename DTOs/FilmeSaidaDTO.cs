using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Models;

namespace CineRank.DTOs
{
    public class FilmeSaidaDTO
    {
        public int Id { get; set; }
        public String Titulo { get; set; } = string.Empty;
        public String Sinopse { get; set; } = string.Empty;
        public String CapaUrl { get; set; } = string.Empty;
        public int AnoLancamento { get; set; }
        public String Genero { get; set; } = string.Empty;
        public double NotaMedia { get; set; }

        public List<FilmeCreditoDTO> Creditos { get; set; } = new List<FilmeCreditoDTO>();
        public List<String> PlataformaNomes { get; set; } = new List<String>();

    }
}