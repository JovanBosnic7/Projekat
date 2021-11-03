using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(NapomenaPosiljkaMetadata))]
    public partial class NapomenaPosiljka
    {
        public class NapomenaPosiljkaMetadata
        {

            public int Id { get; set; }
            public int? PosiljkaId { get; set; }
            public int? PodTipId { get; set; }
            public string TipZadatka { get; set; }
            public string Napomena { get; set; }

            public int? UserUnosId { get; set; }

            public object Posiljka { get; set; }
            public object User { get; set; }
            public object NapomenaPosiljkaPodTip { get; set; }

            private NapomenaPosiljkaMetadata() { }
        }
    }
}