using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniDanStatusMetadata))]
    public partial class ZaposleniDanStatus
    {
        public class ZaposleniDanStatusMetadata
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string NazivSkraceni { get; set; }

            private ZaposleniDanStatusMetadata() { }
        }
    }
}
