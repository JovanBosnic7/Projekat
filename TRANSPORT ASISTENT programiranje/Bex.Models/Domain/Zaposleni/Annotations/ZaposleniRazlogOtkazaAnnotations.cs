using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniRazlogOtkazaMetadata))]
    public partial class ZaposleniRazlogOtkaza
    {
        public class ZaposleniRazlogOtkazaMetadata
        {
           
            public int Id { get; set; }

            public string Sifra { get; set; }

            public string Opis { get; set; }

            private ZaposleniRazlogOtkazaMetadata() { }
        }
    }
}