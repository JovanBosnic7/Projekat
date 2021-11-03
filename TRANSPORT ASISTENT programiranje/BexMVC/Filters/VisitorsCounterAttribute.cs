using System;
using System.Web.Mvc;
using Demos.Club.MVC.Filters.Infrastructure;

namespace Demos.Club.MVC.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method,
        AllowMultiple = false, Inherited = false)]
    public sealed class VisitorsCounterAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);

            Counter.IncreaseVisitorsCounter();
        }
    }
}