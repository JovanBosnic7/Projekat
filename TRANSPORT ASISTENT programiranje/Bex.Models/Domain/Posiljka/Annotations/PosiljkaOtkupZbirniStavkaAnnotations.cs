using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaOtkupZbirniStavkaMetadata))]
    public partial class PosiljkaOtkupZbirniStavka
    {
        public class PosiljkaOtkupZbirniStavkaMetadata
        {

            public int Id { get; set; }
            public string BarKod { get; set; }
            public int? PosiljkaId { get; set; }

            public object PosiljkaOtkupZbirni { get; set; }

            private PosiljkaOtkupZbirniStavkaMetadata() { }
        }
    }
}
