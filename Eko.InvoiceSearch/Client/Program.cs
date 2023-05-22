using CurrieTechnologies.Razor.SweetAlert2;
using Eko.InvoiceSearch.Client;
using Eko.InvoiceSearch.Client.Auth;
using Eko.InvoiceSearch.Client.Repositorios;
using MatBlazor;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
ConfigureServices(builder.Services);

await builder.Build().RunAsync();

async void ConfigureServices(IServiceCollection services)
{
	services.AddScoped<IRepositorios, Repositorio>();
	services.AddAuthorizationCore();
	//1.- La siguiente linea descomentarla para usar autorización sin claim en JWT
	//services.AddScoped<AuthenticationStateProvider, UsuarioAutenticacion>();

	//Comentar las siguiente linea para poder hacer el funcionamiento de la linea 1.-
	services.AddScoped<UsuarioAutenticacionJWT>();
	services.AddScoped<AuthenticationStateProvider, UsuarioAutenticacionJWT>(
		user => user.GetRequiredService<UsuarioAutenticacionJWT>());
	services.AddScoped<ILoginService, UsuarioAutenticacionJWT>(
		user => user.GetRequiredService<UsuarioAutenticacionJWT>());

	services.AddMatBlazor();
	services.AddScoped<DialogService>();
	services.AddScoped<NotificationService>();
	services.AddScoped<TooltipService>();
	services.AddScoped<ContextMenuService>();
	services.AddScoped<RenovadorToken>();
	//services.AddSweetAlert2();

	var host = builder.Build();
	var js = host.Services.GetRequiredService<IJSRuntime>();
	var cultura = await js.InvokeAsync<string>("cultura.get");
	if (cultura == null)
	{
		var culturaDefault = new CultureInfo("es-MX");
		CultureInfo.DefaultThreadCurrentCulture = culturaDefault;
		CultureInfo.DefaultThreadCurrentUICulture = culturaDefault;
	}
	else
	{
		var culturaUsuario = new CultureInfo(cultura);
		CultureInfo.DefaultThreadCurrentCulture = culturaUsuario;
		CultureInfo.DefaultThreadCurrentUICulture = culturaUsuario;
	}
	//var hostLog = builder.Build();
	//var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger<Program>();
	//logger.LogInformation("Looged");
}