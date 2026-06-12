using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class FuncaoCreateDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "O nome da função deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;
    }
}