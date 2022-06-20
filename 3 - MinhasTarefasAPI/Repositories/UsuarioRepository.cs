using _3___MinhasTarefasAPI.Models;
using _3___MinhasTarefasAPI.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {


        private readonly UserManager<ApplicationUser> _userManager;


        public UsuarioRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public void Cadastrar(ApplicationUser usuario, string senha)
        {


          var result =  _userManager.CreateAsync(usuario, senha).Result;

            if (!result.Succeeded)
            {

                StringBuilder sb = new StringBuilder();

                foreach(var erro in result.Errors)
                {
                    sb.Append(erro.Description);

                }

                throw new Exception("Usuario não Localizado " + sb.ToString());
            }
        }

        public ApplicationUser Obter(string email, string senha)
        {

            var usuario = _userManager.FindByEmailAsync(email).Result; //.Result transfrm in sincronous

            if (_userManager.CheckPasswordAsync(usuario, senha).Result)
            {

                return usuario;
            }
            else
            {

                // domain notification better approach instead exception

                throw new Exception("Usuario não Localizado");
            }

        }
    }
}
