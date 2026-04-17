using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        public required string Senha { get; set; }
    }
}