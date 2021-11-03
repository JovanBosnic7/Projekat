using System.Collections.Generic;
using Bex.Models;


namespace BexMVC.ViewModels
{
   public class UserIndexData
   {
        public string AspNetUserId { get; set; }
        public int KontaktId { get; set; }
        //public string KontaktNaziv { get; set; }
      
        public string BarKod { get; set; }
        public int  RegionId { get; set; }
        //public string RegionNaziv { get; set; }
        public bool Klijent { get; set; }
        public bool Aktivan { get; set; }

        public string RoleId { get; set; }

        //public string KorisniciProgramaNaziv { get; set; }
        //public int KorisniciProgramaId { get; set; }


    }
}