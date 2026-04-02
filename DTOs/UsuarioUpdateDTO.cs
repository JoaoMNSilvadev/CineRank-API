using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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