using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(CekoviProvizijaMetadata))]

    public partial class CekoviProvizija
    {
        public class CekoviProvizijaMetadata
        {
            public int Id { get; set; }

            public int Iznos { get; set; }

            private CekoviProvizijaMetadata() { }

        }
    }

}