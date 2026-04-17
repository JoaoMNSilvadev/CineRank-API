using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class FilmeCreateDTO
    {
        [Required (ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "O título deve ter entre 5 e 200 caracteres.")]
        public string Titulo { get; set; } = string.Empty;
        [Required]
        [MinLength(10, ErrorMessage = "A sinopse deve conter pelo menos 10 caracteres.")]
        public string Sinopse { get; set; } = string.Empty;
        [Required]
        [Url(ErrorMessage = "A URL da capa deve ser válida.")]
        public string CapaUrl { get; set; } = string.Empty;
        [Required]
        [Range(1888, 2100, ErrorMessage = "O ano de lançamento deve ser entre 1888 e 2100.")]
        public int AnoLancamento { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "O ID do gênero é obrigatório.")]
        public int GeneroId { get; set; }
        public List<FilmeCreditoInputDTO> Creditos { get; set; } = new List<FilmeCreditoInputDTO>();
            public List<int>? PlataformaIds { get; set; }
        
    }
}