using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class GeneroCreateDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "O nome do gênero deve ter no máximo 100 caracteres.")]
        public string NomeGenero { get; set; } = string.Empty;
    }
}