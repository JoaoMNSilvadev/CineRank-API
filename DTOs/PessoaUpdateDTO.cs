using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class PessoaUpdateDTO
    {
        [StringLength(100, MinimumLength = 2)]
        public string? Nome { get; set; } 
        [StringLength(1000, MinimumLength = 10)]
        public string? Biografia { get; set; } 
        [DataType(DataType.Date)]
        public DateOnly? DataNascimento { get; set; }
        [StringLength(100, MinimumLength = 2)]
        public string? Nacionalidade { get; set; }
        [Url]
        public string? FotoUrl { get; set; }

    }
}