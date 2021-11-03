using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Bex.Models
{
    [MetadataType(typeof(AdresaZaIzborMetadata))]

    public partial class AdresaZaIzbor
    {
        public class AdresaZaIzborMetadata
        {
 
            public int Id{ get; set; }
           
            public string Ulica { get; set; }

            public string Mesto { get; set; }

            
            public string Opstina { get; set; }

           
            public string Adresa { get; set; }

            

            private AdresaZaIzborMetadata() { }

        }
    }

}