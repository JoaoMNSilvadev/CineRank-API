using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.Models
{
    public class Diretor : Pessoa
    {
        public ICollection<Filme>? Filmes { get; set; }
    }
}