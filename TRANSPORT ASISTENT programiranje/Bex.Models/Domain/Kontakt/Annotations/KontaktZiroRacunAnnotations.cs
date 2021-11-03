using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(KontaktZiroRacunMetadata))]
    public partial class KontaktZiroRacun
    {
        public class KontaktZiroRacunMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }

            public string Banka { get; set; }

            public string BrojZiroRacuna { get; set; }

            public object Kontakt { get; set; }

            private KontaktZiroRacunMetadata() { }

        }
    }
}
