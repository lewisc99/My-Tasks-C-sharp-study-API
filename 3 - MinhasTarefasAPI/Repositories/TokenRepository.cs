﻿using _3___MinhasTarefasAPI.Database;
using _3___MinhasTarefasAPI.Models;
using _3___MinhasTarefasAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {


        private readonly MinhasTarefasContext _banco;
        public TokenRepository(MinhasTarefasContext banco)
        {
            _banco = banco;
        }


        public Token obter(string refreshToken)
        {
           return  _banco.Token.FirstOrDefault(a => a.RefreshToken == refreshToken && a.Utilizado == false);

        }


      

        public void Cadastrar(Token token)
        {

            _banco.Token.Add(token);
            _banco.SaveChanges();
        }

        public void Atualizar(Token token)
        {

            _banco.Token.Update(token);
            _banco.SaveChanges();

        }
    }
}
