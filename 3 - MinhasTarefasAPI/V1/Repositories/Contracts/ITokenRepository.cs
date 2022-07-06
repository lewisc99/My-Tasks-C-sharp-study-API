using _3___MinhasTarefasAPI.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.V1.Repositories.Contracts
{
  public interface ITokenRepository
    {

        void Cadastrar(Token token);


        Token obter(string refreshToken);


        void Atualizar(Token token);


    }
}
