using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{

    [MetadataType(typeof(KontaktTelefonMetadata))]
    public partial class KontaktTelefon
    {
        public class KontaktTelefonMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }

           
            public bool Fiksni { get; set; }

            public bool Posao { get; set; }

            [MaxLength(10, ErrorMessage = "KontaktOsoba must be 10 characters or less"), MinLength(5)]
            public string KontaktOsoba { get; set; }

            public string Telefon { get; set; }

            public int UserUnosId { get; set; }

            public object Kontakt { get; set; }

            private KontaktTelefonMetadata() { }

        }
    }
}
