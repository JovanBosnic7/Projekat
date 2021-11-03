using System;
using System.Linq.Expressions;
using Demos.Club.Models;

namespace Demos.Club.Common
{
    public interface IMovieRepository : IRepository<Movie>
    {
        int GetPostsCount(int id, Expression<Func<Post, bool>> predicate = null);
        int GetPostsCount(Movie movie, Expression<Func<Post, bool>> predicate = null);
        IUowCommandResult SqlDelete(int id);
    }
}
