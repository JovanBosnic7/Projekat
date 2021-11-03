using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class UgovorRptCenovnik2Configuration : EntityTypeConfiguration<UgovorRptCenovnik2>
    {
        public UgovorRptCenovnik2Configuration()
        {

            ToTable("vRptCenovnik2");

            HasKey(z => z.IdCene);

            Property(p => p.IdCene)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("IdCene");
        }
    }
}