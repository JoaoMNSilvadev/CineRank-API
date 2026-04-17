using System.ComponentModel.DataAnnotations;

namespace CineRank.DTOs
{
    public class FilmeCreditoInputDTO
    {

    [Required(ErrorMessage = "O ID da pessoa é obrigatório.")]
    public int PessoaId { get; set; }
    [Required(ErrorMessage = "O ID da função é obrigatório.")]
    public int FuncaoId { get; set; }

    }
}
