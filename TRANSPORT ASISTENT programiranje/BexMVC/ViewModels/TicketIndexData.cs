using System.Collections.Generic;
using Bex.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace BexMVC.ViewModels
{
   public class TicketIndexData
    {
        public int Id { get; set; }
        public DateTime DatumUnosa { get; set; }
        public DateTime DatumIzmene { get; set; }
        public string Tekst { get; set; }

        public string UneoNaziv { get; set; }
        public string PrijavioNaziv { get; set; }

        public string Status { get; set; }

        public string NacinPrijave { get; set; }
       
        public string PrihvatioNaziv { get; set; }
       

       


    }
}