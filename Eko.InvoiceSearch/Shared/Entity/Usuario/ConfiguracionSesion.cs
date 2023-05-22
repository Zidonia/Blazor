using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eko.InvoiceSearch.Shared.Entity.Usuario
{
	public class ConfiguracionSesion
	{
		public short TiempoSesion { get; set; }
		public Usuario oUsuario { get; set; }
		public ConfiguracionSesion()
		{
			this.TiempoSesion = 0;
			this.oUsuario = new Usuario();
		}
	}
}
