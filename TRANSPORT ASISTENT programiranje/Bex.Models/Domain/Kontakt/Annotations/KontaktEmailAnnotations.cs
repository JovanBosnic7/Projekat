using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{

    [MetadataType(typeof(KontaktEmailMetadata))]
    public partial class KontaktEmail
    {
        public class KontaktEmailMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }

            public string EmailAdresa { get; set; }

            public int UserUnosId { get; set; }

            public object Kontakt { get; set; }

            private KontaktEmailMetadata() { }

        }
    }
}
