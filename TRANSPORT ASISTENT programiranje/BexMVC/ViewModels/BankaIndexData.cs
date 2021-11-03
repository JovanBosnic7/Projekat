using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BexMVC.ViewModels
{
    public class BankaIndexData
    {
        public int Id { get; set; }
        public DateTime? Datum { get; set; }
        public int? RegionId { get; set; }
        public string RegionNaziv { get; set; }
        public decimal? PazarZaUplatu { get; set; }
        public decimal? PazarUplacen { get; set; }
        public decimal? OtkupZaUplatu { get; set; }
        public decimal? OtkupUplacen { get; set; }
        public decimal? OtkupZaIsplatu { get; set; }
        public decimal? OtkupIsplacen { get; set; }
    }
}