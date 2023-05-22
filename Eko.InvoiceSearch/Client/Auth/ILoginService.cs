using Eko.InvoiceSearch.Shared.DTOs;

namespace Eko.InvoiceSearch.Client.Auth
{
	public interface ILoginService
	{
		//Se va encargar de guardar en local storage el token
		//Task Login(string token);
		////Se va encargar de eliminar el token del local storage
		//Task Logout();
		Task Login(UserToken token);
		Task Logout();
		Task ManejarRenovacionToken();
	}
}
