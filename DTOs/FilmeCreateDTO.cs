using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CineRank.Models;

namespace CineRank.DTOs
{
    public class FilmeCreateDTO
    {
        [Required (ErrorMessage = "O título é obrigatório.")]
        
        public string Titulo { get; set; } = string.Empty;
        [Required]
        [MinLength(10, ErrorMessage = "A sinopse deve conter pelo menos 10 caracteres.")]
        public string Sinopse { get; set; } = string.Empty;
        [Required]
        public string CapaUrl { get; set; } = string.Empty;
        [Required]
        [Range(1888, 2100, ErrorMessage = "O ano de lançamento deve ser entre 1888 e 2100.")]
        public int AnoLancamento { get; set; }
        [Required]
        public int GeneroId { get; set; }
        public List<FilmeCreditoDTO> Creditos { get; set; } = new List<FilmeCreditoDTO>();
        public List<int>? PlataformaIds { get; set; }
        
    }
}