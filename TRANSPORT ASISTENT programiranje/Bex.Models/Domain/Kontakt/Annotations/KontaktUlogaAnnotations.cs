using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{

    [MetadataType(typeof(KontaktUlogaMetadata))]
    public partial class KontaktUloga
    {
        public class KontaktUlogaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Kontakt")]
            public int KontaktId { get; set; }
            [ForeignKey("KontaktUlogaTip")]
            public int UlogaTipId { get; set; }

            
            public object Kontakt { get; set; }
            public object KontaktUlogaTip { get; set; }

            private KontaktUlogaMetadata() { }

        }
    }
}
