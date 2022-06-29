using _3___MinhasTarefasAPI.Models;
using _3___MinhasTarefasAPI.Repositories.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3___MinhasTarefasAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController: ControllerBase
    {

        private readonly IUsuarioRepository _usuarioRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UsuarioController(IUsuarioRepository usuarioRepository, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _usuarioRepository = usuarioRepository;
            _signInManager = signInManager;
            _userManager = userManager;
        }




        [HttpPost("login")]
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


        [HttpPost("")]
        public async Task<ActionResult> Cadastrar([FromBody] UsuarioDTO usuarioDTO )
        {
            if (ModelState.IsValid)
            {

                ApplicationUser usuario = new ApplicationUser();
               
                usuario.UserName = usuarioDTO.Nome;
                usuario.Email = usuarioDTO.Email;
               
                IdentityResult result = await _userManager.CreateAsync(usuario, usuarioDTO.Senha);


                if (!result.Succeeded)
                {
                    List<string> errors = new List<string>();
                    
                    foreach(var error in result.Errors)
                    {
                        errors.Add(error.Description);
                    }
                    return UnprocessableEntity(errors);
                }
                else
                {
                    return Ok(usuario);
                }

            }
            else
            {
                return UnprocessableEntity(ModelState);
            }
        }
    }
}
