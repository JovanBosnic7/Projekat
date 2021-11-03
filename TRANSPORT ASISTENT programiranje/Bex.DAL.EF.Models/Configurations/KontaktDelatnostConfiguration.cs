
namespace Bex.DAL.EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bex.Models;

    public class KontaktDelatnostConfiguration : EntityTypeConfiguration<KontaktDelatnost>
    {
        public KontaktDelatnostConfiguration()
        {
            ToTable("KontaktDelatnost");

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("Id");

            Property(p => p.KontaktId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
                .HasColumnName("KontaktId");

        }
    }


}