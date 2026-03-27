using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string NomeGenero { get; set; } = string.Empty;

        public ICollection<Filme>? Filmes { get; set; }
    }
}