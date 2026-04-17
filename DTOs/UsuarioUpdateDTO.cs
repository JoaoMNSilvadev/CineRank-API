using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class UsuarioUpdateDTO
    {
         [StringLength(100, MinimumLength = 2)]
        public string? Nome { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
    }
}