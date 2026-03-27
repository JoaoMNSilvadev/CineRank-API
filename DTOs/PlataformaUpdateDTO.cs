using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class PlataformaUpdateDTO
    {
        [StringLength(100, MinimumLength = 2)]
        public String? NomePlataforma { get; set; }
        [Url]
        public String? IconeUrl { get; set; }
    }
}