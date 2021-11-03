using System;
using System.Collections.Generic;
using System.Linq;
using Demos.Club.Models;

namespace Bex.DAL.EF.Contexts
{
    public class MovieSeeds
    {
        public IQueryable<Movie> Movies =>
            new List<Movie>
            {
                new Movie
                    {
                        Title = "Avatar",
                        Director ="James Cameron",
                        Distribution =
                            new Distribution
                                {
                                    ReleasedDate = new DateTime(2009, 12, 18),
                                    Duration = 162
                                },
                        Rating = 8M,
                        Mark = MPAA.PG13,
                        Price = 9.99M,
                        Url = "http://www.imdb.com/title/tt0499549/",
                    },
                new Movie
                    {
                        Title = "Harry Potter and the Half-Blood Prince",
                        Director ="David Yates",
                        Distribution =
                            new Distribution
                                {
                                    ReleasedDate = new DateTime(2009, 7, 15),
                                    Tagline = "Once Again I must ask too much of you",
                                    Duration = 153
                                },
                        Rating = 7.3M,
                        Mark = MPAA.PG,
                        Price = 7.99M,
                        Url = "http://www.imdb.com/title/tt0417741/",
                    },
                new Movie
                    {
                        Title = "Twilight",
                        Director ="Catherine Hardwicke",
                        Distribution =
                            new Distribution
                                {
                                    ReleasedDate = new DateTime(2008, 11, 21),
                                    Tagline = "When you can live forever what do you live for",
                                    Duration = 122
                                },
                        Rating = 5.3M,
                        Mark = MPAA.PG13,
                        Price = 4.99M,
                        Url = "http://www.imdb.com/title/tt1099212/",
                    },
            }
            .AsQueryable();
    }
}
