using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Models
{
    public class UsuarioDTO
    {


        [Required]
        public string  Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }


        [Required]
        public string Senha { get; set; }

        [Required]
        [Compare("Senha")] //to compare if ConfirmacaoSenha is equal to password
        public string ConfirmacaoSenha { get; set; }

    }
}
