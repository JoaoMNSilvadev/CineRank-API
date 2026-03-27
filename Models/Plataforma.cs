using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.Models
{
    public class Plataforma
    {
        public int Id { get; set; }
        public string NomePlataforma { get; set; } = string.Empty;
        public string IconeUrl { get; set; } = string.Empty;

        public ICollection<Filme>? Filmes { get; set; }
    }
}