using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Demos.Club.DAL.EF.Models.Common;
using Demos.Club.Models;

namespace Demos.Club.DAL.EF.Models.Correctors
{
    public class MovieCorrector : ICorrector
    {
        public bool IsCorrected(DbEntityEntry entityEntry)
        {
            var movie = (Movie)entityEntry.Entity;

            if (movie.Title == "Twilight")
            { movie.Director = "Catherine Hardwicke"; }

            return true;
        }
    }
}
