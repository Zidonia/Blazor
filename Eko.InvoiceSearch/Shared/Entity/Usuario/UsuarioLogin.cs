using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eko.InvoiceSearch.Shared.Entity.Usuario
{
    public class UsuarioLogin
    {
        public string usuaio { get; set; }
        public string contrasenia { get; set; }
        public UsuarioLogin()
        {
            this.usuaio = string.Empty;
            this.contrasenia = string.Empty;
        }
    }
}
