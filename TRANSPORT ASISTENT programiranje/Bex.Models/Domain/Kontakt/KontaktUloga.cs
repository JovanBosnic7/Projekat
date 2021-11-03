using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KontaktUloga
    {
        public int Id { get; set; }

        public int KontaktId { get; set; }

        public int UlogaTipId { get; set; }

        public string Nadimak { get; set; }
        

        public virtual Kontakt Kontakt { get; set; }
        public virtual KontaktUlogaTip KontaktUlogaTip { get; set; }

    }
}
