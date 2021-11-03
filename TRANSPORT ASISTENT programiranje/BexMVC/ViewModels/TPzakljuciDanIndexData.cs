using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
    public class TPzakljuciDanIndexData
    {
        public decimal? IznosKes { get; set; }
        public decimal? IznosKartica { get; set; }
        public decimal? IznosCek { get; set; }
        public int? LokacijaId { get; set; }
        public int? UserId { get; set; }
    }
}