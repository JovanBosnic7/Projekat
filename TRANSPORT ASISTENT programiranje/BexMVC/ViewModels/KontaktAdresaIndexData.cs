using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class KontaktAdresaIndexData
   {
        public int Id { get; set; }
        public string NazivUlice { get; set; }
        public string NazivMesta { get; set; }
        public string Reon { get; set; }
        public string Region { get; set; }
    }
}