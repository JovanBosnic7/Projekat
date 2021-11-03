using System;

namespace Demos.Club.MVC.Filters.Infrastructure
{
    public static class Counter
    {
        static Counter()
        {
            Visitors = 1;
        }

        public static int Visitors { get; private set; }

        public static int IncreaseVisitorsCounter()
        { return Visitors++; }
    }
}