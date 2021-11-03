using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bex.DAL.EF.Models
{
    public class KovertaZaDokumentConfiguration : EntityTypeConfiguration<KovertaZaDokument>
    {
        public KovertaZaDokumentConfiguration()
        {

            ToTable("vRptKovertaZaDokument");

            HasKey(z => z.Id);

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("Id");
        }
    }
}