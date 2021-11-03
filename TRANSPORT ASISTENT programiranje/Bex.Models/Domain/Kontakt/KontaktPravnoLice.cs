using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    public partial class KontaktPravnoLice
    {
        public KontaktPravnoLice()
        {


        }
        public int Id { get; set; }

        public int KontaktId { get; set; }

        public int PIB { get; set; }

        public string MaticniBroj { get; set; }

        public int DelatnostId { get; set; }

        public string Direktor { get; set; }

        public bool Poslovnica { get; set; }

        public int SedisteKontaktId { get; set; }

        public string WebSajt { get; set; }

        //public byte Logo { get; set; }

        public virtual Kontakt Kontakt { get; set; }

        //public virtual KontaktDelatnost Delatnosti { get; set; }

    }
}
