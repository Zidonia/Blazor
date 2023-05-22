using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Eko.InvoiceSearch.Client.Auth
{
	public class UsuarioAutenticacion: AuthenticationStateProvider
	{
		//Esta clase es un proveedor de autenticación más no uno que indique que es lo que pede hacer el usuario
		public async override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			//await Task.Delay(3000);
			var anonimo = new ClaimsIdentity(
				new List<Claim>{
				//new Claim("llave1","valor1"),
				new Claim(ClaimTypes.Name,"Jonathan"),
				new Claim(ClaimTypes.Role,"admin")
			}, authenticationType: "prueba");
			return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));

			//var anonimo = new ClaimsIdentity();
			//return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonimo)));
		}
	}
}
