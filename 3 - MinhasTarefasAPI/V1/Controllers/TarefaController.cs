using _3___MinhasTarefasAPI.V1.Models;
using _3___MinhasTarefasAPI.V1.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.V1.Controllers
{


    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    [ApiVersion("1.0")]


   
    public class TarefaController:ControllerBase
    {

        private readonly ITarefaRepository _tarefaRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TarefaController(ITarefaRepository tarefaRepository, UserManager<ApplicationUser> userManager)
        {
            _tarefaRepository = tarefaRepository;
            _userManager = userManager;
        }


        //sync add more tasks to the database

        [MapToApiVersion("1.0")]

        [HttpPost("sincronizar")] //httpPOst because can overpass the limit to httpGet

        public ActionResult Sincronizar([FromBody] List<Tarefa> tarefas )
        {

          return Ok( _tarefaRepository.Sincronizacao(tarefas));

        }

   


        //restore will load the tasks served in the database
      
        [HttpGet("restaurar")]
        [MapToApiVersion("1.0")]


        public ActionResult Restaurar(DateTime data)
        {

          var usuario =  _userManager.GetUserAsync(HttpContext.User).Result;


          return Ok( _tarefaRepository.Restauracao(usuario, data));
        
        }

    }
}
