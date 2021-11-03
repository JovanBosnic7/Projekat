using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorNapomenaMetadata))]
    public partial class UgovorNapomena
    {
        public class UgovorNapomenaMetadata
        {
            public int Id { get; set; }
            public int UgovorId { get; set; }
            public int UgovorNapomenaTipId { get; set; }
            public string Napomena { get; set; }
            public DateTime DatumUnosa { get; set; }

            public object Ugovor { get; set; }
            public object UgovorNapomenaTip { get; set; }

            private UgovorNapomenaMetadata() { }
        }
    }
}