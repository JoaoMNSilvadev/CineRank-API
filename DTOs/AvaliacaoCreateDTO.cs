using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class AvaliacaoCreateDTO
    {
        [Required]
        public int UsuarioId { get; set; }
        [Required]
        public int FilmeId { get; set; }
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaHistoria { get; set; }
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaEmocao { get; set; }
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaDirecao { get; set; }
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaTrilha { get; set; }
        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10.")]
        public Double NotaVisual { get; set; }

    }
}