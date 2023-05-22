using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eko.InvoiceSearch.Shared.DTOs
{
    public class UserInfo
    {
        //[Required]
        [EmailAddress]
        public string email { get; set; } = null!;
        //[Required]
        public string password { get; set; } = null!;
    }
}
