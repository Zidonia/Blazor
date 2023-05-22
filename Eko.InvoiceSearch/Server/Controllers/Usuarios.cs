using Eko.InvoiceSearch.Server.Data;
using Eko.InvoiceSearch.Shared.DTOs;
using Eko.InvoiceSearch.Shared.Entity.Usuario;
using Eko.InvoiceSearch.Shared.Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eko.InvoiceSearch.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
	//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class UsuariosController : ControllerBase
    {
        public static SQLConnConfig? _conn;
        public SqlConnection connection;
        public static ReUseSQL? _confQuery;
		private readonly IConfiguration _configuration;

		public UsuariosController(SQLConnConfig conn, IConfiguration configuration)
        {
			Seguridad mSeguridad = new Seguridad();
			_conn = conn;
            connection = new SqlConnection(mSeguridad.DesencriptarDator(_conn.Value.ToString()));
            _confQuery = new ReUseSQL(_conn);
			_configuration = configuration;
		}

        [HttpPost]
        [Route("GetUsuarios")]
        [AllowAnonymous]
        //Con la linea siguiente solo se podrá consumir el método solo usuarios que tengan la autorización
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<Usuario>> ValidaExisteUsuario(UsuarioLogin mLUsuario)
        {
            Usuario mUsuario = new Usuario();
            Seguridad mSeguridad = new Seguridad();

            try
            {
                DataTable dt = new DataTable("Login");
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@user",mLUsuario.usuaio),
                    new SqlParameter("@pwr",mSeguridad.EncriptaDato(mLUsuario.contrasenia))
                };
                dt = _confQuery.GetDataTableStoredProcedure("sp_Login", parameters);

                if (dt.Rows.Count > 0)
                {
                    mUsuario.Idusuario = !DBNull.Value.Equals(dt.Rows[0]["IDUsuario"]) ? long.Parse(dt.Rows[0]["IDUsuario"].ToString()) : 0;
                    mUsuario.User = !DBNull.Value.Equals(dt.Rows[0]["Usuario"]) ? dt.Rows[0]["Usuario"].ToString() : string.Empty;
                    mUsuario.Nombre = !DBNull.Value.Equals(dt.Rows[0]["Nombre"]) ? dt.Rows[0]["Nombre"].ToString() : string.Empty;
                    mUsuario.IdPerfil = !DBNull.Value.Equals(dt.Rows[0]["IDPerfilUsuario"]) ? int.Parse(dt.Rows[0]["IDPerfilUsuario"].ToString()) : 0;
                    mUsuario.Rfc = !DBNull.Value.Equals(dt.Rows[0]["RFC"]) ? dt.Rows[0]["RFC"].ToString() : string.Empty;
                    mUsuario.PwrActualizado = !DBNull.Value.Equals(dt.Rows[0]["PwrActualizado"]) ? int.Parse(dt.Rows[0]["PwrActualizado"].ToString()) : 0;
                }
			}
            catch (Exception ex)
            {

                var te = ex.Message.ToString();
            }
            return mUsuario;
        }
        [HttpPost]
        [Route("CrearToken")]
        [AllowAnonymous]
		public async Task<ActionResult<UserToken>> BuildToken(Usuario usuario)
		{
			//Los claims son información de un usuario en la que podemos confiar, nada de información sencible
			var claims = new[] {
				new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName,usuario.Nombre),
				new Claim(ClaimTypes.Name, usuario.Nombre),
				//new Claim("miValor","Lo que yo quiera"),
				new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
			};

			//Parte de seguridad
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			//Timepo de expiración al token
            //Aqui le indicamos que el toquen se caduca cada minuto
			var expiration = DateTime.UtcNow.AddHours(5);
            //Aqui se va a renovar en automatico siempre y cuando el usuario este logueado

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

        [HttpGet]
        [Route("RenovarToken")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<UserToken>> Renovar() {
			var usuario = new Usuario()
			{
				Nombre = HttpContext.User.Identity!.Name!
			};
			return await BuildToken(usuario);
		}

        [HttpPost]
        [Route("TestMethod")]
		[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public async Task<ActionResult<string>> PruebaMetodo(UsuarioLogin usuario) {
            string nombre = string.Empty;
            try
            {
                nombre = "Jonathan";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return nombre;
        }
		
	}
}
