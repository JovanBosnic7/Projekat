using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorPovlascenaCenaTipMetadata))]
    public partial class UgovorPovlascenaCenaTip
    {
        public class UgovorPovlascenaCenaTipMetadata
        {

            public int Id { get; set; }
            public string PovlascenaCenaTip { get; set; }

            private UgovorPovlascenaCenaTipMetadata() { }
        }
    }
}