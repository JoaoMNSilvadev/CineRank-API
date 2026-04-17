using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class PaginacaoDTO<T>
    {
        public int Pagina { get; set; }
        public int Quantidade { get; set; }
        public int Total { get; set; }
        public int TotalPaginas { get; set; }
        public List<T> Dados { get; set; } = new List<T>();
    }
}