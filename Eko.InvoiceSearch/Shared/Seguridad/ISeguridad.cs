using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eko.InvoiceSearch.Shared.Seguridad
{
	public interface ISeguridad
	{
		public string CaracteresInvalidos(string cadenaTexto);
		public string PathTraversal(string cadena);
		public string EncriptaDato(string dato);
		public string DesencriptarDator(string dato);
	}
}
