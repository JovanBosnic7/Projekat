using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(UgovorNacinNaplateMetadata))]
    public partial class UgovorNacinNaplate
    {
        public class UgovorNacinNaplateMetadata
        {

            public int Id { get; set; }
            public string NazivNaplate { get; set; }


            private UgovorNacinNaplateMetadata() { }
        }
    }
}