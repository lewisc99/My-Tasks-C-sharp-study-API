using _3___MinhasTarefasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Repositories.Contracts
{
    public interface IUsuarioRepository
    {

        //password encrypted
        void Cadastrar(ApplicationUser usuario, string senha);

        ApplicationUser Obter(string email, string senha);


    }
}
