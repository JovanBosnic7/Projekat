using System.Collections.Generic;
using Bex.Models;


namespace BexMVC.ViewModels
{
   public class StreetIndexData
   {
        public IEnumerable<Street> Street { get; set; }
        public Place Place { get; set; }

        public int UlicaId { get; set; }
        public string NazivMesta { get; set; }
        public string NazivUlice { get; set; }
    }
}