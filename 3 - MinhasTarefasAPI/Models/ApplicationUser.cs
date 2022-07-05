using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Models
{



    //application user has the own properties created by the system.
    //and we added more two fullName and Tarefas
    public class ApplicationUser: IdentityUser
    {

       


        [ForeignKey("UsuarioId")]


        public virtual ICollection<Tarefa> Tarefas { get; set; }


        [ForeignKey("UsuarioId")]

        public virtual ICollection<Token> Tokens { get; set; }
    }
}
