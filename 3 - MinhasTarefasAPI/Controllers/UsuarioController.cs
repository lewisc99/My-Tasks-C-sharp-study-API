using _3___MinhasTarefasAPI.Models;
using _3___MinhasTarefasAPI.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController: ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UsuarioController(IUsuarioRepository usuarioRepository, SignInManager<ApplicationUser> signInManager)
        {
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
        }





        public ActionResult Login([FromBody] UsuarioDTO usuarioDTO)
        {

            ModelState.Remove("Nome");
            ModelState.Remove("ConfirmacaoSenha");

            if(ModelState.IsValid)
            {
              ApplicationUser usuario =  _usuarioRepository.Obter(usuarioDTO.Email, usuarioDTO.Senha);
              
                if (usuario != null)
                {
                    //login no identity
                    //user, then cookie parameters.
                    _signInManager.SignInAsync(usuario, false);


                    //no futuro retorna o Token (JWT)
                    return Ok();
                }
                else
                {
                    return NotFound("Usuario Não Localizado");
                }
            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }
    }
}
