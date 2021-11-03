using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Bex.Models
{
    [MetadataType(typeof(KontaktMetadata))]

    public partial class Kontakt
    {
        public class KontaktMetadata
        {
 
            public int Id{ get; set; }


            [Required(ErrorMessage = "Ime/Naziv je obavezan podatak.")]
            public string Ime { get; set; }

           
            public string Prezime { get; set; }

            [DatabaseGenerated(DatabaseGeneratedOption.Computed)]

            public string Naziv { get; set; }

            
            public int PakId { get; set; }

            [Required(ErrorMessage = "Broj je obavezan podatak. Uneti bb ako nema.")]
            public string BrojTxt { get; set; }

            public bool PravnoLice { get; set; }

            public bool Stranac { get; set; }

            [Required(ErrorMessage = "Adresa je obavezan podatak.")]
            public string AdresaTekst { get; set; }

            //public int AdresaId { get; set; }

           
            public int FirmaUnosId { get; set; }
            public int UserUnosId { get; set; }

            //public object Adresa { get; set; }

            private KontaktMetadata() { }

        }
    }

}