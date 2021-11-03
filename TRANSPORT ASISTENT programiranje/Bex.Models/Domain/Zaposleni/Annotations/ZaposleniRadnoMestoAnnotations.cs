using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Bex.Models
{
    [MetadataType(typeof(ZaposleniRadnoMestoMetadata))]
    public partial class ZaposleniRadnoMesto
    {
        public class ZaposleniRadnoMestoMetadata
        {
           
            public int Id { get; set; }

            public string InterniNaziv { get; set; }

            public string SistematizacijaNaziv { get; set; }

            private ZaposleniRadnoMestoMetadata() { }
        }
    }
}