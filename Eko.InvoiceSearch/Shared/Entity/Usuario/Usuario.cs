using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eko.InvoiceSearch.Shared.Entity.Usuario
{
    public class Usuario
    {
        public long Idusuario { get; set; }
        public string User { get; set; }
        public string Contrasenia { get; set; }
        public string Nombre { get; set; }
        public int IdPerfil { get; set; }
        public string Rfc { get; set; }
        public int PwrActualizado { get; set; }

        public Usuario()
        {
            this.Idusuario= 0;
            this.User= string.Empty;
            this.Contrasenia= string.Empty;
            this.Nombre = string.Empty; 
            this.Rfc = string.Empty;
            this.PwrActualizado= 0;
        }
    }
}
