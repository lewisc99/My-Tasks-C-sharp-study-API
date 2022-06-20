﻿using _3___MinhasTarefasAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Database
{
    public class MinhasTarefasContext :IdentityDbContext
    {

        public MinhasTarefasContext(DbContextOptions<MinhasTarefasContext> options) :base(options)
        {

        }


        public DbSet<Tarefa> Tarefas { get; set; }

    }
}
        