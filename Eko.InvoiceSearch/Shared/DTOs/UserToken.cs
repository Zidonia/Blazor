using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eko.InvoiceSearch.Shared.DTOs
{
    public class UserToken
    {
        public string token { get; set; } = null!;
        public DateTime expiration { get; set; }
    }
}
