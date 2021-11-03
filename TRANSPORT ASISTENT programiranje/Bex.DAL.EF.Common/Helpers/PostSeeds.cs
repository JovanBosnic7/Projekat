using System;
using System.Collections.Generic;


namespace Bex.DAL.EF.Contexts
{
    public class PostSeeds
    {
        public IEnumerable<Post> GetSome(int movieId, string title, int length)
        {
            for (int i = 0; i < length; i++)
            {
                yield return new Post
                {
                    MovieId = movieId,
                    Title = $"{title} post {i + 1}",
                    Content =
                        (i % 2 == 0) ?
                        $"Content of {title} post {i + 1}." :
                        null
                };
            }
        }
    }
}
