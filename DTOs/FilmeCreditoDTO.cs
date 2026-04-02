using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class FilmeCreditoDTO
    {

    public int PessoaId { get; set; }
    public string NomePessoa { get; set; } = string.Empty; // Novo campo
    public int FuncaoId { get; set; }
    public string NomeFuncao { get; set; } = string.Empty; // Novo campo
}
    }
