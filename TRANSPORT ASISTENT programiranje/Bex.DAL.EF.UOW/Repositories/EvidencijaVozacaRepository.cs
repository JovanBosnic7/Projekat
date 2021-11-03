using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Bex.DAL.EF.UOW
{
    public class EvidencijaVozacaRepository : BaseRepository<EvidencijaVozaca>, IEvidencijaVozacaRepository
    {
        public EvidencijaVozacaRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public EvidencijaVozacaRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }



        public IUowCommandResult EvidencijaImport(int zaposleniId,string dan, DateTime datum, string start, string stop, string radniDan,string dnevnoRadnoVreme, string upravljanje,string ostalo, string raspolozivost,string odmor, string odsustvo, string noc)
        {
            var sqlString = @"EXEC prEvidencijaImport " + zaposleniId.ToString() + ",'" + dan + "'," + Datumtoformatzabazu(datum) + ",'" + start + "','" + stop + "','" + radniDan + "','" + dnevnoRadnoVreme + "','" + upravljanje + "','" + ostalo + "','" + raspolozivost + "','" + odmor + "','" + odsustvo + "','" + noc + "'";

            return UowCommandResultFactory.Invoke(() =>
            {
                return DataContext.Database.ExecuteSqlCommand(sqlString, zaposleniId.ToString() + ",'" + dan + "'," + Datumtoformatzabazu(datum) + ",'" + start + "','" + stop + "','" + radniDan + "','" + dnevnoRadnoVreme + "','" + upravljanje + "','" + ostalo + "','" + raspolozivost + "','" + odmor + "','" + odsustvo + "','" + noc + "'");
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
