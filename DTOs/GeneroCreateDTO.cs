using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class GeneroCreateDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "O nome do gênero deve ter no máximo 100 caracteres.")]
        public string NomeGenero { get; set; } = string.Empty;
    }
}