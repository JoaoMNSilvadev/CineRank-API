using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class AvaliacaoCreateDTO
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int FilmeId { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaHistoria { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaEmocao { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaDirecao { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaTrilha { get; set; }
        [Required]
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaVisual { get; set; }

        public AvaliacaoCreateDTO()
        {
        }
    }
}