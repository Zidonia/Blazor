using CurrieTechnologies.Razor.SweetAlert2;
using Eko.InvoiceSearch.Client.Auth;
using Eko.InvoiceSearch.Client.Helpers;
using Eko.InvoiceSearch.Client.Repositorios;
using Eko.InvoiceSearch.Shared.DTOs;
using Eko.InvoiceSearch.Shared.Entity.Usuario;
using Eko.InvoiceSearch.Shared.Seguridad;
using MatBlazor;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Radzen;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Eko.InvoiceSearch.Client.Pages.Login
{
	public partial class Login
	{
		#region Inyección Dependencia
		[Inject] IRepositorios repositorio { get; set; } = null!;
		[Inject] ILoginService LoginService { get; set; } = null!;
		[Inject] NavigationManager NavManager { get; set; } = null!;
		[Inject] NotificationService NotificationService { get; set; } = null!;
		#endregion
		#region Variables
		public string usuario = string.Empty;
		public string password = string.Empty;
		public MatTheme temaPrincipal;
		public bool popup;
		private Notificacion notificacion = new Notificacion();
		#endregion

		protected override void OnInitialized()
		{
			//temaPrincipal = new MatTheme()
			//{
			//	Primary = MatThemeColors.LightBlue._500.Value,
			//	Secondary = MatThemeColors.Amber._500.Value,
			//	Surface = MatThemeColors.Cyan._500.Value,
			//};
			usuario = string.Empty;
			password = string.Empty;
			Navigation.NavigateTo($"/login");
		}

		[JSInvokable]
		public async Task Ingreso()
		{
			#region Variables
			Usuario mUsuario = new Usuario();
			Seguridad mSeguridad = new Seguridad();
			#endregion
			try
			{
				if (!ValidaUsuarioContrasenia())
				{
					return;
				}

				usuario = mSeguridad.CaracteresInvalidos(usuario);
				password = mSeguridad.CaracteresInvalidos(password);

				var responseHTTP = await repositorio.Post<UsuarioLogin, Usuario>("api/usuarios/getusuarios", new UsuarioLogin { usuaio = usuario, contrasenia = password });

				if (!responseHTTP.Error)
				{
					mUsuario = responseHTTP.Response;
					if (mUsuario.Idusuario == 0)
					{
						NotificationService.Notify(notificacion.MensajeAlerta("Usuario y/o contraseña incorrectos"));
					}
					else
					{
						var reponseToken = await repositorio.Post<Usuario, UserToken>("api/usuarios/creartoken", mUsuario);
						await LoginService.Login(reponseToken.Response!);
						NavManager.NavigateTo("/index");
					}
				}
				else
				{
					NotificationService.Notify(notificacion.MensajeError(responseHTTP.responseMessage.StatusCode.ToString()));
				}
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
			}
		}
		[JSInvokable]
		public async Task RecuperarPass()
		{
			Console.WriteLine("Recuperar contraseña");
		}

		#region Complementos
		public bool ValidaUsuarioContrasenia()
		{
			bool bandera = true;
			try
			{
				if (usuario == "" || password == "")
				{
					bandera = false;
					NotificationService.Notify(notificacion.MensajeAlerta("El campo Usuario y Contraseña son requeridos"));
				}
			}
			catch (Exception ex)
			{

			}
			return bandera;
		}
		public bool ContraseniaActualizada(int esActualizado)
		{
			bool bandera = true;
			try
			{
				switch (esActualizado)
				{
					case -2:
						NotificationService.Notify(notificacion.MensajeInfo("Manda a formulario NuevoUsuario"));
						break;
					case -1:
						NotificationService.Notify(notificacion.MensajeInfo("Usuario con correo."));
						break;
					default:
						bandera = false;
						break;
				}
			}
			catch (Exception ex)
			{
				bandera = false;
			}
			return bandera;
		}
		#endregion
	}
}
