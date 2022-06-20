﻿using _3___MinhasTarefasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Repositories.Contracts
{
    public interface ITarefaRepository
    {

        // Create tasks
        List<Tarefa> Sincronizacao(List<Tarefa> tarefas );


        //read the tasks
        List<Tarefa> Restauracao(ApplicationUser usuario, DateTime DataUltimaSincronizacao);
    }
}
