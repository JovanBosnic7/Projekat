using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VozniParkDnevnikTipMetadata))]
    public partial class VozniParkDnevnikTip
    {
        public class VozniParkDnevnikTipMetadata
        {

            public int Id { get; set; }
            public int GrupaId { get; set; }
            public string NazivTipa { get; set; }

            public int? DanaIstekaDefault { get; set; }
            public int? KmIstekaDefault { get; set; }
            private VozniParkDnevnikTipMetadata() { }
        }
    }
}
