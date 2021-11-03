using System.Collections.Generic;
using Bex.Models;


namespace BexMVC.ViewModels
{
   public class PlaceIndexData
   {
        public IEnumerable<Place> Place { get; set; }
        public Opstina Opstina { get; set; }

        public IEnumerable<Street> Street { get; set; }

        public int MestoId { get; set; }
        public string NazivMesta { get; set; }
        public string NazivOpstine { get; set; }
        public string Ptt { get; set; }

    }
}