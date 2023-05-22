using Microsoft.JSInterop;

namespace Eko.InvoiceSearch.Client.Helpers
{
	public static class IJSRuntimeExtensionMethods
	{
		//public static async ValueTask<bool> Confirm(this IJSRuntime js, string mensaje)
		//{
		//	await js.InvokeVoidAsync("console.log", "prueba de console.log");
		//	return await js.InvokeAsync<bool>("confirm", mensaje);
		//}

		public static ValueTask<object> GuardarEnLocalStorage(this IJSRuntime js, string key, string content)
			=> js.InvokeAsync<object>("localStorage.setItem", key, content);

		public static ValueTask<string> ObtenerDeLocalStorage(this IJSRuntime js, string key)
			=> js.InvokeAsync<string>("localStorage.getItem", key);
		public static ValueTask<object> RemoverDeLocalStorage(this IJSRuntime js, string key)
			=> js.InvokeAsync<object>("localStorage.removeItem", key);

		//Permite guardar la llave en cache de la máquina del usuario
		//public static ValueTask<object> GuardarEnLocalStorage(this IJSRuntime js,
		//	string llave, string contenido)
		//{
		//	return js.InvokeAsync<object>("localStorage.setItem", llave, contenido);
		//}

		//public static ValueTask<object> ObtenerDeLocalStorage(this IJSRuntime js,
		//	string llave)
		//{
		//	return js.InvokeAsync<object>("localStorage.getItem", llave);
		//}

		//public static ValueTask<object> RemoverDeLocalStorage(this IJSRuntime js,
		//	string llave)
		//{
		//	return js.InvokeAsync<object>("localStorage.removeItem", llave);
		//}

	}
}
