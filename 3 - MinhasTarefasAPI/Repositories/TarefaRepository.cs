using _3___MinhasTarefasAPI.Database;
using _3___MinhasTarefasAPI.Models;
using _3___MinhasTarefasAPI.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Repositories
{
    public class TarefaRepository : ITarefaRepository
    {

        private readonly MinhasTarefasContext _banco;


        public TarefaRepository(MinhasTarefasContext banco )
        {
            _banco = banco;

        }


        public List<Tarefa> Restauracao( ApplicationUser usuario,DateTime DataUltimaSincronizacao)
        {

            var query = _banco.Tarefas.Where(a => a.UsuarioId == usuario.Id).AsQueryable();
             

            if (DataUltimaSincronizacao != null)
            {
                query.Where(a => a.Criado >= DataUltimaSincronizacao || a.Atualizado >= DataUltimaSincronizacao);

            }

            return query.ToList<Tarefa>();


        }

        
        // Tarefa IdTarefaAPI - App IdTarefaAPI = Tarefa Local
        public List<Tarefa> Sincronizacao(List<Tarefa> tarefas)
        {
            //Cadastrar novos registros



            var novasTarefas = tarefas.Where(a => a.IdTarefaApi == 0);
            if (novasTarefas.Count() > 0)
            {
                //task receive from API
                foreach (var tarefa in novasTarefas)
                {
                    _banco.Tarefas.Add(tarefa);

                }
                _banco.SaveChanges();

            }


            //Atualizacao de registro (Excluido)
            var tarefasExcluidasAtualizadas = tarefas.Where(a => a.IdTarefaApi != 0);

            if (tarefasExcluidasAtualizadas.Count() > 0)
            {
                //task receive from API
                foreach (var tarefa in tarefasExcluidasAtualizadas)
                {
                    _banco.Tarefas.Update(tarefa);

                }
               

            }


            _banco.SaveChanges();

            return novasTarefas.ToList();

        }
    }
}
