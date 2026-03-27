using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class PessoaCreateDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nome { get; set; } = string.Empty;
        [StringLength(1000, MinimumLength = 10)]
        public string Biografia { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DataNascimento { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 2)]

        public string Nacionalidade { get; set; } = string.Empty;
        [Url]
        public string FotoUrl { get; set; } = string.Empty;
    }
}