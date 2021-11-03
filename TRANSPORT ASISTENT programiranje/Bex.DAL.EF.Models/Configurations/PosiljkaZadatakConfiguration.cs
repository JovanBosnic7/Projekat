using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using Bex.Models;

namespace Bex.DAL.EF.Models
{
    public class PosiljkaZadatakConfiguration: EntityTypeConfiguration<PosiljkaZadatak>
    {
        public PosiljkaZadatakConfiguration()
        {
            ToTable("Zadatak");

            HasKey(e => e.Id);

            Property(e => e.Id)
                .HasColumnName("IDzadatka");

            Property(e => e.Adresa)
                .HasColumnName("UlicaNaziv");

            HasOptional(e => e.Posiljka)
           .WithMany(e => e.PosiljkaZadatak)
           .HasForeignKey(e => e.PosiljkaId)
           .WillCascadeOnDelete(false);

            HasOptional(e => e.Reon)
            .WithMany(e => e.PosiljkaZadatak)
            .HasForeignKey(e => e.ReonId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.Kontakt) //tetka zakomentarisala, ccko vratio. treba pogledati
            .WithMany(e => e.PosiljkaZadatak)
            .HasForeignKey(e => e.SubId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.User) //tetka zakomentarisala, ccko vratio. treba pogledati
            .WithMany(e => e.PosiljkaZadatak)
            .HasForeignKey(e => e.IzvrsioId)
            .WillCascadeOnDelete(false);

            HasOptional(e => e.PAK)
           .WithMany(e => e.PosiljkaZadatak)
           .HasForeignKey(e => e.PakId)
           .WillCascadeOnDelete(false);
        }
    }
}