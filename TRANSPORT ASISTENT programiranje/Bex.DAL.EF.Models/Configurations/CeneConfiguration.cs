using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class CeneConfiguration : EntityTypeConfiguration<Cene>
    {
        public CeneConfiguration()
        {
            ToTable("Cena");

            HasKey(e => e.IdCene);

            Property(e => e.IdCene)
                .HasColumnName("IdCene");

        }
    }
}