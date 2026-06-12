using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class FavoritoSaidaDTO
    {
        public int UsuarioId { get; set; }
        public int FilmeId { get; set; }

        public string? Titulo { get; set; }
    }
}