using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KontaktPravnoLiceMetadata))]
    public partial class KontaktPravnoLice
    {
        public class KontaktPravnoLiceMetadata 
        {

            public int Id { get; set; }

            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }

            public int PIB { get; set; }

            public string MaticniBroj { get; set; }

            public int DelatnostId { get; set; }

            public string Direktor { get; set; }

            public bool Poslovnica { get; set; }

            //public int sedisteKontaktId { get; set; }

            public string WebSajt { get; set; }

            //public byte Logo { get; set; }

            public object Kontakt { get; set; }

            private KontaktPravnoLiceMetadata() { }
        }

    }
}
