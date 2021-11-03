using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bex.Models
{
    [MetadataType(typeof(KurirRazduzenjeMetadata))]
    public partial class KurirRazduzenje
    {
        public class KurirRazduzenjeMetadata
        {
            public int Id { get; set; }
            public int? KurirId { get; set; }
            public int? ReonId { get; set; }

            public decimal? UkupnoOtkupa { get; set; }

            public decimal? UkupnoPazara { get; set; }

            public decimal? UkupnoNaplatio { get; set; }

            public decimal? UkupnoRazduzio { get; set; }

            public decimal? Razlika { get; set; }

            public string Napomena { get; set; }

            public int? UserUnosId { get; set; }

            public DateTime Datum { get; set; }


            public object Reon { get; set; }
            public object UserKurir { get; set; }
            

            private KurirRazduzenjeMetadata() { }
        }
    }
}
