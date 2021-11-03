using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(StetaTipMetadata))]
    public partial class StetaTip
    {
        public class StetaTipMetadata
        {

            public int Id { get; set; }
            public int KategorijaId { get; set; }
            public string NazivTipa { get; set; }
            public bool Storno { get; set; }

            private StetaTipMetadata() { }
        }
    }
}
