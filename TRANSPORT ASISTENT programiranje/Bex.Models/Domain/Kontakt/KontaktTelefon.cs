namespace Bex.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public partial class KontaktTelefon
    {
        public KontaktTelefon()
        {

            
        }
        public int Id { get; set; }

        public int KontaktId { get; set; }

        public bool Fiksni { get; set; }

        public bool Posao { get; set; }

        public string KontaktOsoba { get; set; }

        public string Telefon { get; set; }

        public int UserUnosId { get; set; }

        public virtual Kontakt Kontakt { get; set; }
    }
}
