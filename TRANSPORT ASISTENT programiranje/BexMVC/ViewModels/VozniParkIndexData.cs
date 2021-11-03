using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class VozniParkIndexData
   {
        public int Id { get; set; }
        public string Model { get; set; }
        public string GarazniBroj { get; set; }
        public string Oznaka { get; set; }

        public string Region { get; set; }
        public int? RegionId { get; set; }
        public string Status { get; set; }

        public string Marka { get; set; }
        public string Opis { get; set; }

        public string Sasija { get; set; }

        public int? Godiste { get; set; }

        public string Napomena { get; set; }

        public DateTime? DatumRegistracije { get; set; }
        public DateTime? DatumZaduzenja { get; set; }

        public int? BrojZone { get; set; }

        public string FirmaOS { get; set; }

        public string Namena { get; set; }

        public string Barkod { get; set; }

        public decimal? PropisanaPotrosnja { get; set; }

        public string Sektor { get; set; }
        public string Kategorija { get; set; }

        public string NazivPodkategorije { get; set; }
        public string KategorijaSaPodkategorijom { get; set; }
        public string OznakaStara { get; set; }
        public string TipVozila { get; set; }
        public DateTime? DatumFakturisanja { get; set; }
        public string VrstaGoriva { get; set; }


        public IEnumerable<VozniParkDnevnik> VozniParkDnevnik { get; set; }

    }
}