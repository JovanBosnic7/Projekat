using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PosiljkaNapomenaMetadata))]
    public partial class PosiljkaNapomena
    {
        public class PosiljkaNapomenaMetadata
        {

            public int Id { get; set; }
            [ForeignKey("Posiljka")]
            public int PosiljkaId { get; set; }
            [ForeignKey("PosiljkaNapomenaTip")]
            public int NapomenaTipId { get; set; }
            public int Napomena { get; set; }

            public object Posiljka { get; set; }
            public object PosiljkaNapomenaTip { get; set; }

            private PosiljkaNapomenaMetadata() { }
        }
    }
}