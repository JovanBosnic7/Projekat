using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class KovertaZaOtkupninuConfiguration : EntityTypeConfiguration<KovertaZaOtkupninu>
    {
        public KovertaZaOtkupninuConfiguration()
        {

            ToTable("vRptKovertaZaOtkupninu");

            HasKey(z => z.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("Id");
        }
    }
}