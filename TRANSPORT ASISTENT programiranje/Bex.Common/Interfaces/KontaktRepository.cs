using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Bex.Common
{
    public class KontaktRepository : IKontaktRepository, IDisposable
    {
        private AddressesDbContext context;

        public KontaktRepository(AddressesDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Kontakt> GetKontakts()
        {
            return context.Kontakt.ToList();
        }

        public Kontakt GetKontaktByID(int kontaktId)
        {
            return context.Kontakt.Find(kontaktId);
        }

        public void InsertKontakt(Kontakt kontakt)
        {
            context.Kontakt.Add(kontakt);
        }

        public void DeleteKontakt(int kontaktId)
        {
            Kontakt kontakt = context.Kontakt.Find(kontaktId);
            context.Kontakt.Remove(kontakt);
        }

        public void UpdateKontakt(Kontakt kontakt)
        {
            context.Entry(kontakt).State = System.Data.Entity.EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<KontaktAdresa> GetKontaktAdresa(int kontaktId)
        {
            IEnumerable<KontaktAdresa> kontaktAdrese = context.KontaktAdresa.Where(
                                      i => i.KontaktId == kontaktId);
            return kontaktAdrese;
        }

        public KontaktPravnoLice GetKontaktPravnaLica(int kontaktId)
        {
            KontaktPravnoLice kontaktPravnoLice = context.KontaktPravnaLica.Where(
                         i => i.KontaktId == kontaktId).Single();

            return kontaktPravnoLice;
        }

        public KontaktFizickoLice GetKontaktFizickaLica(int kontaktId)
        {
            KontaktFizickoLice kontaktFizickoLice = context.KontaktFizickoLice.Where(
                     i => i.KontaktId == kontaktId).Single();

            return kontaktFizickoLice;
        }

        public IEnumerable<KontaktTelefon> GetKontaktTelefon(int kontaktId)
        {
            IEnumerable<KontaktTelefon> kontaktTelefon = context.KontaktTelefon.Where(
                                     i => i.KontaktId == kontaktId);
            return kontaktTelefon;
        }

        public IEnumerable<KontaktEmail> GetKontaktEmail(int kontaktId)
        {
            IEnumerable<KontaktEmail> kontaktEmail = context.KontaktEmail.Where(
                                     i => i.KontaktId == kontaktId);
            return kontaktEmail;
        }

        public IEnumerable<KontaktZiroRacun> GetKontaktZiroRacun(int kontaktId)
        {
            IEnumerable<KontaktZiroRacun> kontaktZiroRacun = context.KontaktZiroRacun.Where(
                                     i => i.KontaktId == kontaktId);
            return kontaktZiroRacun;
        }

        public IEnumerable<KontaktDelatnost> GetKontaktDelatnost(int kontaktId)
        {
            IEnumerable<KontaktDelatnost> kontaktDelatnost = context.KontaktDelatnost.Where(
                         i => i.KontaktId == kontaktId);
            return kontaktDelatnost;
        }
    }
}
