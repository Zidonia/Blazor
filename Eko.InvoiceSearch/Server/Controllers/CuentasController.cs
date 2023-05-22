using Eko.InvoiceSearch.Shared.DTOs;
using Eko.InvoiceSearch.Shared.Entity.Usuario;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eko.InvoiceSearch.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentasController : ControllerBase
	{
        //private readonly UserManager<UsuarioLogin> _userManager;
        private readonly SignInManager<UsuarioLogin> _signInManager;
        private readonly IConfiguration _configuration;

		//public CuentasController(UserManager<UsuarioLogin> userManager,
		//    SignInManager<UsuarioLogin> signInManager,
		//    IConfiguration configuration)
		//{
		//    _userManager = userManager;
		//    _signInManager = signInManager;
		//    _configuration = configuration;
		//}
		public CuentasController(IConfiguration configuration, SignInManager<UsuarioLogin> signInManager)
		{
			_configuration = configuration;
			_signInManager = signInManager;
		}

		[HttpPost]
        [Route("Crear")]
		public async Task<ActionResult<UserToken>> CreateUser(UsuarioLogin model)
		{
			var user = new UsuarioLogin { usuaio = model.usuaio, contrasenia = model.contrasenia };
			//var result = await _userManager.CreateAsync(user, model.contrasenia);
			return BuildToken(model);
			//if (result.Succeeded)
   //         {
   //             return BuildToken(model);
   //         }
   //         else
   //         {
   //             return BadRequest("Usuario o contraseña invalidos");
   //         }
		}

        private UserToken BuildToken(UsuarioLogin userInfo)
        {
            //Los claims son información de un usuario en la que podemos confiar, nada de información sencible
            var claims = new[] {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName,userInfo.usuaio),
                new Claim(ClaimTypes.Name, userInfo.usuaio),
                new Claim("miValor","Lo que yo quiera"),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };

            //Parte de seguridad
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //Timepo de expiración al token
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);

            return new UserToken()
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UsuarioLogin userInfo)
        {
			//El isPersistent es para las coockes
			//Si el usuariseequioca muchas veces la cuenta se puede cerrar lockoutOnFailure
			var result = await _signInManager.PasswordSignInAsync(userInfo.usuaio, 
                userInfo.contrasenia, 
                isPersistent: false, 
                lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return BuildToken(userInfo);
            }
            else
            {
                return BadRequest("Intento de login invalido");
            }
        }

    }
}
