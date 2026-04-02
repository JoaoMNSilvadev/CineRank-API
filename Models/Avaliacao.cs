using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public Double NotaHistoria { get; set; }
        public Double NotaEmocao { get; set; }
        public Double NotaDirecao { get; set; }
        public Double NotaTrilha { get; set; }
        public Double NotaVisual { get; set; }

        public int FilmeId { get; set; }
        public Filme? Filme { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

       public Double NotaFinal { get; set; }

       public DateTime DataAvaliacao { get; set; } = DateTime.Now;

    }
}