using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.Models
{
    public class Pessoa
    {
         
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Biografia { get; set; } = string.Empty;
        public DateOnly DataNascimento { get; set; }
        public string Nacionalidade { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;

        public ICollection<Filme>? FilmesDiretor { get; set; }
        public ICollection<Filme>? FilmesAtor { get; set; }
    }
}