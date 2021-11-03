using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
    public class TPocitavanjaIndexData
    {
        public int Id { get; set; }
        public string KategorijaVozila { get; set; }
        public string KaroserijaVozila { get; set; }
        public string Model { get; set; }
        public string BrojSasije { get; set; }
        public string BrojMotora { get; set; }
        public string Boja { get; set; }
        public int? BrojVrata { get; set; }
        public string Kontrolor { get; set; }
        public DateTime? DatumOcitavanja { get; set; }
        public string Uneo { get; set; }
        public string Napomena { get; set; }
    }
}