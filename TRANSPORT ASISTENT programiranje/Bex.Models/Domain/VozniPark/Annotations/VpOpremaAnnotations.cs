using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(VpOpremaMetadata))]
    public partial class VpOprema
    {
        public class VpOpremaMetadata
        {
            public int Id { get; set; }
            [ForeignKey("VozniPark")]
            public int VpId { get; set; }
            [ForeignKey("VpOpremaPodTip")]
            public int PodTipId { get; set; }
            [ForeignKey("KorisniciPrograma")]
            public int UserUnosId { get; set; }
            public DateTime DatumUnosa { get; set; }

            public object VpOpremaPodTip { get; set; }
            public object VozniPark { get; set; }
            public object KorisniciPrograma { get; set; }

            private VpOpremaMetadata() { }
        }
    }
}