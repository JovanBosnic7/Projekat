using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TrebovanjeMetadata))]

    public partial class Trebovanje
    {
        public class TrebovanjeMetadata
        {
            public int? IdTrebovanja { get; set; }
            public DateTime? DatumTrebovanja { get; set; }
            [ForeignKey("Kontakt")]
            public int? SubTrazioId { get; set; }
            [ForeignKey("UserIzdao")]
            public int? UserIzdaoId { get; set; }
            public DateTime? DatumIzdavanja { get; set; }
            [ForeignKey("UserUneo")]
            public int? UserUneoId { get; set; }
            public bool? Arhivirano { get; set; }
            public bool? Storno { get; set; }

            public object Kontakt { get; set; }
            public object UserUneo { get; set; }
            public object UserIzdao { get; set; }

            private TrebovanjeMetadata() { }

        }
    }

}