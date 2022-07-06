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

     
        [HttpPost("sincronizar")] //httpPOst because can overpass the limit to httpGet
        public ActionResult Sincronizar([FromBody] List<Tarefa> tarefas )
        {

          return Ok( _tarefaRepository.Sincronizacao(tarefas));

        }

     
        [HttpGet("olamundo")]
        [AllowAnonymous]
        public ActionResult OlaMundo()
        {
            return Ok("ola mundo");
        }


        [HttpGet("helloworld")]
        public ActionResult HelloWorld()
        {
            return Ok("hello world");
        }


        //restore will load the tasks served in the database
      
        [HttpGet("restaurar")]
        
        public ActionResult Restaurar(DateTime data)
        {

          var usuario =  _userManager.GetUserAsync(HttpContext.User).Result;


          return Ok( _tarefaRepository.Restauracao(usuario, data));
        
        }

    }
}
