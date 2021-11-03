using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class GorivoRepository : BaseRepository<GorivoTocenje>, IGorivoRepository
    {
        public GorivoRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public GorivoRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }



        public IUowCommandResult GorivoImport(string registracija="", decimal kolicina=0, int km=0, decimal cena=0, string vreme="", DateTime? datum=null, int pumpaId=0)
        {
            var sqlString = @"EXEC prGorivoImport " + "'" + registracija + "'," + kolicina.ToString() + "," + km.ToString() + "," + cena.ToString() + ",'" + vreme + "'," + Datumtoformatzabazu(datum) + "," + pumpaId.ToString();

            return UowCommandResultFactory.Invoke(() =>
            {
                return DataContext.Database.ExecuteSqlCommand(sqlString, registracija + "," + kolicina.ToString() + "," + km.ToString() + "," + cena.ToString() + "," + vreme + "," + Datumtoformatzabazu(datum) + "," + pumpaId.ToString());
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
