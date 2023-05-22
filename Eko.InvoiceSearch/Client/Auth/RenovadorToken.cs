using System.Timers;
using Timer = System.Timers.Timer;

namespace Eko.InvoiceSearch.Client.Auth
{
	public class RenovadorToken: IDisposable
	{
		public ILoginService LoginService { get; }
		Timer? timer;
		public RenovadorToken(ILoginService loginService)
		{
			LoginService = loginService;
		}
		public void Iniciar() {
			timer = new Timer();
			timer.Interval= 1000*60*4;//4 minutos Nota: El tiempo que se coloca aqui debe ser menor al umbral que se coloca en UsuarioAutenticacionJWT
			//timer.Interval = 1000 * 5;//Cada 5 segundos
			timer.Elapsed += Timer_Elapsed;
			timer.Start();
		}

		private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
		{
			LoginService.ManejarRenovacionToken();
		}

		public void Dispose() { 
			//Contiene recursos no manejados y debemos controlarlos nosotros
			timer?.Dispose();
		}
	}
}
