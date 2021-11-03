using System;


namespace Bex.Common
{
    public class CourseRepository : GenericRepository<Kontakt>
    {
        public CourseRepository(AddressesDbContext context)
            : base(context)
        {
        }

       /* public int UpdateCourseCredits(int multiplier)
        {
            return context.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
        }*/

    }
}
