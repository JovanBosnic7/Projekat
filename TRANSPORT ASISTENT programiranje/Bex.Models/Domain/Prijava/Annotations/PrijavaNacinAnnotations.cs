using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(PrijavaNacinMetadata))]
    public partial class PrijavaNacin
    {
        public class PrijavaNacinMetadata
        {

            public int Id { get; set; }          
            public string Naziv { get; set; }
            private PrijavaNacinMetadata() { }
        }
    }
}