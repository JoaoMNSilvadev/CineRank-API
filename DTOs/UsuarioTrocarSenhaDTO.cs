using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CineRank.DTOs
{
    public class UsuarioTrocarSenhaDTO
    {
     
    [Required]
    public string SenhaAtual { get; set; } = string.Empty;

    [Required]
    [StringLength(25, MinimumLength = 8, ErrorMessage = "A nova senha deve ter entre 8 e 25 caracteres.")]
    public string NovaSenha { get; set; } = string.Empty;

    [Compare("NovaSenha", ErrorMessage = "A confirmação não coincide com a nova senha.")]
    public string ConfirmarNovaSenha { get; set; } = string.Empty;
}
    }
