using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class FilmeUpdateDTO
    {
        [StringLength(200, MinimumLength = 5)]
        public string? Titulo { get; set; }
        [StringLength(500, MinimumLength = 10)]
        public string? Sinopse { get; set; }    
        [Url(ErrorMessage = "A URL da capa deve ser válida.")]
        public string? CapaUrl { get; set; }
        [Range(1888, 2100, ErrorMessage = "O ano de lançamento deve ser entre 1888 e 2100.")]
        public int? AnoLancamento { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "O ID do gênero é obrigatório.")]
        public int? GeneroId { get; set; }
        public List<int>? PlataformaIds { get; set; }
        public List<FilmeCreditoInputDTO>? Creditos { get; set; }

    }
}