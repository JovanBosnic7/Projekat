using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.Models
{
    [MetadataType(typeof(TPocitavanjaMetadata))]

    public partial class TPocitavanja
    {
        public class TPocitavanjaMetadata
        {
            public int Id { get; set; }
            [ForeignKey("KategorijaVozila")]
            public int? KategorijaVozilaId { get; set; }
            [ForeignKey("KaroserijaVozila")]
            public int? KaroserijaVozilaId { get; set; }
            [ForeignKey("ModelVozila")]
            public int? ModelId { get; set; }
            public string BrojSasije { get; set; }
            public string BrojMotora { get; set; }
            [ForeignKey("BojaVozila")]
            public int? BojaId { get; set; }
            public int? BrojVrata { get; set; }
            [ForeignKey("Kontrolor")]
            public int? KontrolorId { get; set; }
            public DateTime? DatumOcitavanja { get; set; }
            [ForeignKey("UserUnosa")]
            public int? UserUneoId { get; set; }
            public string Napomena { get; set; }
            public bool? Storno { get; set; }

            public object KategorijaVozila { get; set; }
            public object KaroserijaVozila { get; set; }
            public object ModelVozila { get; set; }
            public object BojaVozila { get; set; }
            public object Kontrolor { get; set; }
            public object UserUnosa { get; set; }

            private TPocitavanjaMetadata() { }

        }
    }

}