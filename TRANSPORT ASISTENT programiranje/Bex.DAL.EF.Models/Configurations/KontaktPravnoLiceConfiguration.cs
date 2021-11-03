
namespace Bex.DAL.EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bex.Models;

    public class KontaktPravnoLiceConfiguration : EntityTypeConfiguration<KontaktPravnoLice>
    {
        public KontaktPravnoLiceConfiguration()
        {
            ToTable("KontaktPravnoLice");

            Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)// has to be but in db will be non-identity
                .HasColumnName("Id");

            Property(p => p.KontaktId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)// by default
                .HasColumnName("KontaktId");

            //HasRequired(c => c.Kontakt)
            //.WithMany(p => p.Pravno)
            //.HasForeignKey(c => c.KontaktId)
            //.WillCascadeOnDelete();
            //HasOptional(e => e.Delatnosti)
            //    .WithMany(e => e.KontaktPravnoLice)
            //    .HasForeignKey(e => e.DelatnostId)
            //    .WillCascadeOnDelete(false);

            //MapToStoredProcedures(map => map
            //    .Delete(d => d.HasName("DeletePost")
            //        .Parameter(p => p.Key, "PostID"))
            //    .Insert(i => i.HasName("InsertPost")));


        }
    }


}