using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TicketKategorijaMetadata))]
    public partial class TicketKategorija
    {
        public class TicketKategorijaMetadata
        {
            public int Id { get; set; }
            [ForeignKey("Sektor")]
            public int SektorId { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public object Sektor { get; set; }
            private TicketKategorijaMetadata() { }
        }
    }
}