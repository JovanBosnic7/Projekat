using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class UgovorRptConfiguration : EntityTypeConfiguration<UgovorRpt>
    {
        public UgovorRptConfiguration()
        {

            ToTable("vRptUgovor");

            HasKey(z => z.IdUgovora);

            Property(p => p.IdUgovora)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("IdUgovora");
        }
    }
}