using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class StatistikaPorekloPosiljkeConfiguration: EntityTypeConfiguration<StatistikaPorekloPosiljke>
    {
        public StatistikaPorekloPosiljkeConfiguration()
        {
            ToTable("StatistikaPorekloPosiljke");

            HasKey(e => e.Id);

            Property(e => e.Sat)
                .HasColumnName("SatZaDijagram");

        }
    }
}