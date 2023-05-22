using Eko.InvoiceSearch.Server.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
var SQLConncfig = new SQLConnConfig(builder.Configuration.GetConnectionString("DBConexion_Extraccion")!);
builder.Services.AddSingleton(SQLConncfig);
builder.Services.AddServerSideBlazor(x => x.DetailedErrors = true);
//La siguiente configuración permite la autenticación en el proyecto,Json web token, permite enviar a cualquier proyecto las credenciales de 
//usuarios para su autenticación
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //No valida emisores
        ValidateIssuer = false,
        //No valida receptores
        ValidateAudience = false,
        //Tiempo de vida de un token, puede ser valido por 1hr o X tiempo
        ValidateLifetime = true,
        //Valida la llave de firma del emisor
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]!)),
        ClockSkew = TimeSpan.Zero
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
//Los siguientes dos sirven para el login Json Wtoken
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
//app.MapDefaultControllerRoute();
app.MapFallbackToFile("index.html");

app.Run();
