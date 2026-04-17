using System.ComponentModel.DataAnnotations;

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