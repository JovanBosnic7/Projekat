using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KontaktDelatnost
    {
        public int Id { get; set; }

        public int KontaktId { get; set; }

        public int SifraDelatnosti { get; set; }

        public string NazivDelatnosti { get; set; }

        public int UserUnosId { get; set; }

        public virtual Kontakt Kontakt { get; set; }

        //public virtual ICollection<KontaktPravnoLice> KontaktPravnoLice { get; set; }
    }
}
