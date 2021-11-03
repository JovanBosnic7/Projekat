using System;
using System.Data.Entity;
using Bex.Models;
using Bex.Common;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Bex.DAL.EF.UOW
{
    public class KontaktAdresaRepository : BaseRepository<KontaktAdresa>, IKontaktAdresaRepository
    {
        public KontaktAdresaRepository(DbContext dbContext) :
                    this(dbContext, new UowCommandResultFactory())
        { }
        public KontaktAdresaRepository(
            DbContext dbContext, IUowCommandResultFactory uowCommandResultFactory) : base(dbContext)
        {
            UowCommandResultFactory = uowCommandResultFactory;
        }
        
        

        //public IEnumerable<KontaktAdresa> GetKontaktAdresaData()
        //{
        //    var kontaktAdresa = DataSet.Include(g => g.Adresa);
        //    return kontaktAdresa;
        //}

       

        private IUowCommandResultFactory UowCommandResultFactory { get; }

        public IEnumerable<KontaktAdresa> GetKontaktAdresaData()
        {
            throw new NotImplementedException();
        }
    }
}
