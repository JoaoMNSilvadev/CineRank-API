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
        [StringLength(100, ErrorMessage = "O nome da plataforma deve ter no máximo 100 caracteres.")]
        public string NomePlataforma { get; set; } = string.Empty;
        [Required(ErrorMessage = "A URL do ícone é obrigatória.")]
        [Url(ErrorMessage = "A URL do ícone deve ser válida.")]
        public string IconeUrl { get; set; } = string.Empty;
    }
}