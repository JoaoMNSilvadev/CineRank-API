using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class PlataformaCreateDTO
    {
        [Required(ErrorMessage = "O nome da plataforma é obrigatório.")]
        [StringLength(100, MinimumLength = 2)]
        public string NomePlataforma { get; set; } = string.Empty;
        [Url]
        public string IconeUrl { get; set; } = string.Empty;
    }
}