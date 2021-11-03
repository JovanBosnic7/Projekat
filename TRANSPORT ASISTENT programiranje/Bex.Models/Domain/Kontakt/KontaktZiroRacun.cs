using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KontaktZiroRacun
    {
        public int Id { get; set; }

        public int KontaktId { get; set; }

        public string Banka { get; set; }

        public string BrojZiroRacuna { get; set; }

        public virtual Kontakt Kontakt { get; set; }

    }
}
