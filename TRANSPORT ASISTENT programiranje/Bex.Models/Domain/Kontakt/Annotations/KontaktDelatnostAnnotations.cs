using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{

    [MetadataType(typeof(KontaktDelatnostMetadata))]
    public partial class KontaktDelatnost
    {
        public class KontaktDelatnostMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }

            public int SifraDelatnosti { get; set; }

            public string NazivDelatnosti { get; set; }

            public int UserUnosId { get; set; }

            public object Kontakt { get; set; }

            private KontaktDelatnostMetadata() { }

        }
    }
}
