using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorNapomenaTipMetadata))]
    public partial class UgovorNapomenaTip
    {
        public class UgovorNapomenaTipMetadata
        {
            public int Id { get; set; }
            public string NazivTipa { get; set; }

            private UgovorNapomenaTipMetadata() { }
        }
    }
}