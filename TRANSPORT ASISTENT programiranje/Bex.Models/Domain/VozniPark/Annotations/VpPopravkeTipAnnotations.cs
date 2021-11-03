using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(VpPopravkeTipMetadata))]
    public partial class VpPopravkeTip
    {
        public class VpPopravkeTipMetadata
        {

            public int Id { get; set; }
            public string Naziv { get; set; }

            private VpPopravkeTipMetadata() { }
        }
    }
}
