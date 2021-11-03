using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KontaktEmail
    {
        public int Id { get; set; }

        public int KontaktId { get; set; }

        public string EmailAdresa { get; set; }

        public int UserUnosId { get; set; }

        public virtual Kontakt Kontakt { get; set; }
    }
}
