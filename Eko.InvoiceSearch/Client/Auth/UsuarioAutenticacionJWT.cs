using Eko.InvoiceSearch.Client.Helpers;
using Eko.InvoiceSearch.Client.Repositorios;
using Eko.InvoiceSearch.Shared.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace Eko.InvoiceSearch.Client.Auth
{
	public class UsuarioAutenticacionJWT : AuthenticationStateProvider, ILoginService
	{
		public static readonly string TOKENKEY = "TOKENKEY";
		public static readonly string EXPIRATIONTOKENKEY = "EXPIRATIONTOKENKEY";
		private readonly IJSRuntime js;
		private readonly HttpClient httpClient;
		private readonly IRepositorios repositorios;
		private AuthenticationState anonimo => 
			new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
		public UsuarioAutenticacionJWT(IJSRuntime js, HttpClient httpClient, IRepositorios repositorios)
		{
			this.js = js;
			this.httpClient = httpClient;
			this.repositorios = repositorios;
		}
		//Este metodo permite identificar si un usuario esta autentificado mediante un token
		//El token lo vamos a guardar en localstoresh
		public async override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var token = await js.ObtenerDeLocalStorage(TOKENKEY);
			if (string.IsNullOrEmpty(token))
			{
				return anonimo;
			}

			//Se valida si el tiempo del token es valido
			var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(EXPIRATIONTOKENKEY);
			DateTime tiempoExpiracion;
			if (tiempoExpiracionObject is null) {
				await Limpiar();
				return anonimo;
			}
			if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion)) {
				if (TokenExpirado(tiempoExpiracion)) { 
					await Limpiar();
					return anonimo;
				}
				if (DebeRenovarToken(tiempoExpiracion)) {
					token = await RenovarToken(token.ToString()!);
				}
			}

			return ConstruirAuthenticationState(token);
		}
		private bool TokenExpirado(DateTime tiempoExpiracion) { 
			return tiempoExpiracion <= DateTime.UtcNow;
		}
		private bool DebeRenovarToken(DateTime tiempoExpiracion) { 
			return tiempoExpiracion.Subtract(DateTime.UtcNow) < TimeSpan.FromMinutes(5);
		}

		public async Task ManejarRenovacionToken() {
			var tiempoExpiracionObject = await js.ObtenerDeLocalStorage(EXPIRATIONTOKENKEY);
			DateTime tiempoExpiracion;
			if (DateTime.TryParse(tiempoExpiracionObject.ToString(), out tiempoExpiracion)) {
				if (TokenExpirado(tiempoExpiracion)) { 
					await Logout();
				}
				if (DebeRenovarToken(tiempoExpiracion)) {
					var token = await js.ObtenerDeLocalStorage(TOKENKEY);
					var nuevoToken = await RenovarToken(token.ToString()!);
					var authState = ConstruirAuthenticationState(nuevoToken);
					//Como se obtiene el token, puede que eset tenga nuevos claims
					NotifyAuthenticationStateChanged(Task.FromResult(authState));
				}
			}
		}
		private async Task<string> RenovarToken(string token) { 
			Console.WriteLine("Renovando el token...");
			httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("bearer", token);
			var nuevoTokenResponse = await repositorios.Get<UserToken>("api/usuarios/renovartoken");
			var nuevoToken = nuevoTokenResponse.Response!;

			await js.GuardarEnLocalStorage(TOKENKEY, nuevoToken.token);
			await js.GuardarEnLocalStorage(EXPIRATIONTOKENKEY, nuevoToken.expiration.ToString());
			return nuevoToken.token;
		}
		//Metodo auxiliar donde se recibe el token
		private AuthenticationState ConstruirAuthenticationState(string token)
		{
			//Tiene tiempo de vida tipo Singleton
			//bearer persona que lleva algo
			httpClient.DefaultRequestHeaders.Authorization = 
				new AuthenticationHeaderValue("bearer", token);
			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
		}
		//Este metodo trabaja con los claims del cliente
		private IEnumerable<Claim> ParseClaimsFromJwt(string token)
		{
			var claims = new List<Claim>();
			var payload = token.Split('.')[1];
			var jsonBytes = ParseBase64WithoutPadding(payload);
			var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

			keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

			if (roles != null)
			{
				if (roles.ToString().Trim().StartsWith("["))
				{
					var parseRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());
					foreach (var parseRole in parseRoles)
					{
						claims.Add(new Claim(ClaimTypes.Role, parseRole));
					}
				}
				else
				{
					claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
				}
				keyValuePairs.Remove(ClaimTypes.Role);
			}
			claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
			return claims;
		}
		//Metodo auxiliar para obtener los claims
		private byte[] ParseBase64WithoutPadding(string base64)
		{
			switch (base64.Length % 4)
			{
				case 2: base64 += "=="; break;
				case 3: base64 += "="; break;
			}
			return Convert.FromBase64String(base64);
		}

		//Guarda en local Storage el token del usuario
		//public async Task Login(string token)
		//{
		//	await js.GuardarEnLocalStorage(TOKENKEY, token);
		//	var authState = ConstruirAuthenticationState(token);
		//	//Este es importante a blazor que el estado de autenticación a cambiado y por lo tanto los componentes como authoresview se deben actualizar
		//	//para ver si el usuario tiene permitido ver los compenentes que se han restringido
		//	NotifyAuthenticationStateChanged(Task.FromResult(authState));
		//}
		public async Task Login(UserToken token)
		{
			await js.GuardarEnLocalStorage(TOKENKEY, token.token);
			await js.GuardarEnLocalStorage(EXPIRATIONTOKENKEY, token.expiration.ToString());
			var authState = ConstruirAuthenticationState(token.token);
			//Este es importante a blazor que el estado de autenticación a cambiado y por lo tanto los componentes como authoresview se deben actualizar
			//para ver si el usuario tiene permitido ver los compenentes que se han restringido
			NotifyAuthenticationStateChanged(Task.FromResult(authState));
		}

		//Se elimina un elemento de local storage
		public async Task Logout()
		{
			await Limpiar();
			//await js.RemoverDeLocalStorage(TOKENKEY);
			//httpClient.DefaultRequestHeaders.Authorization = null;
			NotifyAuthenticationStateChanged(Task.FromResult(anonimo));
		}

		//Limpia la data del JWToken
		private async Task Limpiar() {
			await js.RemoverDeLocalStorage(TOKENKEY);
			await js.RemoverDeLocalStorage(EXPIRATIONTOKENKEY);
			httpClient.DefaultRequestHeaders.Authorization = null;
		}

	}
}
