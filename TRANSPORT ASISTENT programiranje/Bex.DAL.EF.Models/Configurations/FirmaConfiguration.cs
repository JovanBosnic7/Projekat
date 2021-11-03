using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class FirmaConfiguration: EntityTypeConfiguration<Firma>
    {
        public FirmaConfiguration()
        {
            ToTable("Firma");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IdFirme");
        }
    }
}