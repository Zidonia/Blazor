using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eko.InvoiceSearch.Shared.Entity
{
    public class Idioma
    {
        public string valor { get; set; }
        public string descripcion { get; set; }

        public Idioma()
        {
            this.valor = string.Empty;
            this.descripcion = string.Empty;
        }

    }
}
