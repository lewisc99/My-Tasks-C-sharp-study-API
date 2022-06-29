using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Models
{
    public class LoginDTO
    {


     
        [UIHint("email")]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        [UIHint("email")]
        public string Email { get; set; }


        [Required]
        [UIHint("Senha")]
        public string Senha { get; set; }

        [Required]
        [Compare("Senha")]
        public string ConfirmacaoSenha { get; set; }


    }
}
