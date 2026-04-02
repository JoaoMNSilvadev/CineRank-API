using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class AvaliacaoSaidaDTO
    {
        public double NotaHistoria { get; set; }
        public double NotaEmocao { get; set; }
        public double NotaDirecao { get; set; }
        public double NotaTrilha { get; set; }
        public double NotaVisual { get; set; }
        public double NotaFinal { get; set; }
        public DateTime DataAvaliacao { get; set; }
    }
}