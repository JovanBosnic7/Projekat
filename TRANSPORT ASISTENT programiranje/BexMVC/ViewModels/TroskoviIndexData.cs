using System.Collections.Generic;
using Bex.Models;


namespace BexMVC.ViewModels
{
   public class TroskoviIndexData
   {
        public IEnumerable<VozniParkDnevnik> VozniParkDnevnik { get; set; }
        public VozniPark VozniPark { get; set; }
    }
}