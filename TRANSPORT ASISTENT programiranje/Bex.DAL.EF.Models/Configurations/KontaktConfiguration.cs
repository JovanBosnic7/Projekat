
namespace Bex.DAL.EF.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Data.Entity.ModelConfiguration;
    using System.ComponentModel.DataAnnotations.Schema;
    using Bex.Models;

    public class KontaktConfiguration : EntityTypeConfiguration<Kontakt>
    {
        public KontaktConfiguration()
        {
            HasKey(e => e.Id);

            ToTable("Kontakt");

            Property(e => e.Id)
                .HasColumnName("IdKontakt");

            Property(e => e.AdresaTekst)
                .HasColumnName("Adresa");


           // HasRequired(c => c.Adresa)
           //.WithMany(p => p.Kontakt)
           //.HasForeignKey(c => c.AdresaId)
           //.WillCascadeOnDelete();



        }
    }


}