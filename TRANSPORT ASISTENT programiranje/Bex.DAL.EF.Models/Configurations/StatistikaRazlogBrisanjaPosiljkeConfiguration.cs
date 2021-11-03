using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class StatistikaRazlogBrisanjaPosiljkeConfiguration: EntityTypeConfiguration<StatistikaRazlogBrisanjaPosiljke>
    {
        public StatistikaRazlogBrisanjaPosiljkeConfiguration()
        {
            ToTable("StatistikaRazlogBrisanjaPosiljke");

            HasKey(e => e.Id);

            Property(e => e.Sat)
                .HasColumnName("SatZaDijagram");

        }
    }
}