using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.Models
{
    public class Credito
    {

        public int Id { get; set; }
        public int PessoaId { get; set; }
        public Pessoa? Pessoa { get; set; }

        public int FuncaoId { get; set; }
        public Funcao? Funcao { get; set; }

        public int FilmeId { get; set; } 
        public Filme? Filme { get; set; }
    }
}