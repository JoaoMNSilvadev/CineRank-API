using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public int AnoLancamento { get; set; }
        [Required]

        public int GeneroId { get; set; }
        [Required]
        public int DiretorId { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaHistoria { get; set; }
        [Required]
        [Range(0, 10)]
        public Double NotaEmocao { get; set; }
        [Required]
        [Range(0, 10)]
        public Double NotaDirecao { get; set; }
        [Required]
        [Range(0, 10)]
        public Double NotaTrilha { get; set; }
        [Required]
        [Range(0, 10)]
        public Double NotaVisual { get; set; }

        public List<int>? PlataformaIds { get; set; }
        public List<int>? AtorIds { get; set; }
    }
}