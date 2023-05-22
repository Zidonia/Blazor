using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Eko.InvoiceSearch.Shared.Seguridad
{
	public class Seguridad : ISeguridad
	{
		public string CaracteresInvalidos(string cadenaTexto)
		{
			string cadena = cadenaTexto;
			try
			{
				cadena = cadena.Replace("'", "");
				cadena = cadena.Replace(";", "");
				cadena = cadena.Replace("/", "");
				cadena = cadena.Replace("\\", "");
				cadena = cadena.Replace("$", "");
			}
			catch (Exception)
			{
				return cadena;
			}
			return cadena;
		}

		public string DesencriptarDator(string dato)
		{
			return "";
		}

		public string EncriptaDato(string dato)
		{
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			string Clave = string.Empty;
			int i = 0;
			string valuePass = string.Empty;
			string CAR = string.Empty;
			string Codigo = string.Empty;

			try
			{
				Clave = "%ü&/@#$A";
				var loopTo = Strings.Len(dato);

				for (i = 1; i <= loopTo; i++)
				{
					CAR = Strings.Mid(dato, i, 1);
					Codigo = Strings.Mid(Clave, (i - 1) % Strings.Len(Clave) + 1, 1);
					
					System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
					valuePass = valuePass + Strings.Right("0" + Conversion.Hex(Strings.Asc(Codigo) ^ Strings.Asc(CAR)), 2);
				}
			}
			catch (Exception)
			{
				return valuePass;
			}
			return valuePass;
		}

		public string PathTraversal(string cadenaTexto)
		{
			string cadena = string.Empty;
			try
			{
				cadena = HttpUtility.UrlDecode(cadenaTexto);

				if (cadena.Contains("/"))
					cadena = cadena.Replace("/", "");
				if (cadena.Contains("\\"))
					cadena = cadena.Replace("\\", "");
				if (cadena.Contains("$"))
					cadena = cadena.Replace("$", "");
				if (cadena.Contains(".."))
					cadena = cadena.Replace("..", "");
				if (cadena.Contains("?"))
					cadena = cadena.Replace("?", "");
			}
			catch (Exception)
			{
				return cadena;
			}
			return cadena;
		}

	}
}
