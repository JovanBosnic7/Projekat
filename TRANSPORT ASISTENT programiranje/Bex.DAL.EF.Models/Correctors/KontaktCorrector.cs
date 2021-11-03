using Bex.DAL.EF.Models;
using Bex.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace Bex.DAL.EF.Models.Correctors
{
    public class KontaktCorrector : ICorrector
    {
        public bool IsCorrected(DbEntityEntry entityKontakt)
        {
            var kontakt = (Kontakt)entityKontakt.Entity;

            if (entityKontakt.State == EntityState.Added) //|| kontakt.CreatedDate == DateTime.MinValue && kontakt.Adresa == null 
            {
                //kontakt.CreatedDate = DateTime.Now.Date; 
            }

            //kontakt.DatumIzmene = DateTime.Now;

            return true;
        }
    }
}
