using _3___MinhasTarefasAPI.Models;
using _3___MinhasTarefasAPI.Repositories.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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




        /*  [HttpPost("login")]
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
          } */

        [HttpPost("login")]
        public async Task<ActionResult> login([FromBody] LoginDTO loginDTO)
        {

            ModelState.Remove("Nome");
            ModelState.Remove("ConfirmacaoSenha");


            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (user != null)
                {
                    //await _signInManager.SignOutAsync();
                   
                    //Microsoft.AspNetCore.Identity.SignInResult result =
                    //    await _signInManager.PasswordSignInAsync(
                    //        user, loginDTO.Senha, false, false);

                    //if (result.Succeeded)
                    //{
                        return Ok(BuildToken(user));
                   // }
                }
                ModelState.AddModelError(nameof(loginDTO.Email),
                    "Invalid user or password");
            }

            return Unauthorized();
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

        [Authorize]
        [HttpGet("logout")]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("user sign Out");
        }

        public object BuildToken(ApplicationUser usuario)
        {

            var claims = new[]
            {
              // new Claim(JwtRegisteredClaimNames.Aud, "wwww.meuapp.com.br") to use when a website need a permission
              new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
              new Claim(JwtRegisteredClaimNames.Sub, usuario.Id) //this property helps identify the user.

           };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes( "chave-api-jwt-minhas-tarefas")); // recommend  add the text in -> appsetttings.json
            var sign = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var exp = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer:null,
                audience:null, //means which site is requiring the token, null add to any
                claims: claims,
                expires:exp,
                signingCredentials: sign);


            var tokenString = new JwtSecurityTokenHandler().WriteToken(token); // passing the token datas and will generate a string encrypted 


            return new { token = tokenString, expiration = exp };

        }

    }


   
}
