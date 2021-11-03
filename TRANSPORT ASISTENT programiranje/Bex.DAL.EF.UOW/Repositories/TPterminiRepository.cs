using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class TPterminiRepository : BaseRepository<TPtermini>, ITPterminiRepository
    {
        public TPterminiRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public TPterminiRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }


        
        public IUowCommandResult ZakljuciDan(decimal? iznosKes = 0, decimal? iznosKartica = 0, decimal? iznosCek = 0, int? lokacijaId = 0, int? userId = 0)
        {
            var sqlString = @"EXEC [prZakljuciDanTP] " + iznosKes.ToString() + "," + iznosKartica.ToString() + "," + iznosCek.ToString() + "," + lokacijaId.ToString() + "," + userId.ToString();

            return UowCommandResultFactory.Invoke(() =>
            {
                return DataContext.Database.ExecuteSqlCommand(sqlString, iznosKes.ToString() + "," + iznosKartica.ToString() + "," + iznosCek.ToString() + "," + lokacijaId.ToString() + "," + userId.ToString());
            });
        }

        private static string Datumtoformatzabazu(DateTime? datum)
        {
            string dat="NULL";
            if (datum.HasValue == true)
            {
                dat = datum.Value.Year + @"-" + Stringdopunisleva(datum.Value.Month.ToString(), 2, "0") + @"-" + Stringdopunisleva(datum.Value.Day.ToString(), 2, "0");                
            }
            return "'" + dat + "'";
        }

        private static string Stringdopunisleva(string str, int ukupnoznakova, string znakzadopunjavanje)
        {
            if (str.Length < ukupnoznakova)
            {
                str = Stringistiznakovi(ukupnoznakova - str.Length, znakzadopunjavanje) + str;
            }
            return str;
        }

        private static string Stringistiznakovi(int brojznakova, string znak)
        {
            string x = "";
            if (znak.Length <= 0) znak = " ";
            while (x.Length < brojznakova) x += znak;
            return x;
        }

        private IUowCommandResultFactory UowCommandResultFactory { get; }
    }
}
