using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(PutniNalogTipMetadata))]
    public partial class PutniNalogTip
    {
        public class PutniNalogTipMetadata
        {

            public int Id { get; set; }

            public string NazivTipa { get; set; }

            public bool Vestacki { get; set; }

            public bool Storno { get; set; }

            private PutniNalogTipMetadata() { }
        }
    }
}
