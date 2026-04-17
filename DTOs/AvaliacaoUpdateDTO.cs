using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class AvaliacaoUpdateDTO
    {
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public double? NotaHistoria { get; set; }

        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public double? NotaEmocao { get; set; }

        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public double? NotaDirecao { get; set; }

        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public double? NotaTrilha { get; set; }

        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public double? NotaVisual { get; set; }
    }
}