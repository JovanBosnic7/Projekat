using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{

    [MetadataType(typeof(KontaktUlogaTipMetadata))]
    public partial class KontaktUlogaTip
    {
        public class KontaktUlogaTipMetadata
        {

            public int Id { get; set; }
           
            public string Naziv { get; set; }
            public string Opis { get; set; }

            
            private KontaktUlogaTipMetadata() { }

        }
    }
}
