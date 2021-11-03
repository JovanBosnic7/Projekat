
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace BexMVC.Filters
{
    [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method,
        Inherited = true, AllowMultiple = true)]
    public class ClaimsAuthenticationAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        public string Resource { get; set; }
        public string Operation { get; set; }

        public void OnAuthentication(AuthenticationContext filterContext)
        {
            //In real app you can use custom principal and later test it
            //filterContext.Principal = new MyCustomPrincipal{ ... };
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            var checkAccess = false;

            var principal = filterContext.HttpContext.User as IPrincipal;
            var claimsIdentity = principal.Identity as ClaimsIdentity;

            Func<Claim, string> ClaimType = claim =>
                claim.Type.Split('/').Last();

            Func<Claim, string> ClaimValue = claim =>
                claim.Type.EndsWith("sid") ?
                new SecurityIdentifier(claim.Value)
                    .Translate(typeof(NTAccount)).Value :
                claim.Value;

            var typeValues = claimsIdentity.Claims
                .Select(claim => new
                {
                    Type = ClaimType(claim),
                    Value = ClaimValue(claim)
                })
                .Where(typeValue => typeValue.Type == Resource);

            var operations = Operation.Split(',');

            checkAccess = typeValues.Any(typeValue =>
                operations.Any(operation =>
                    typeValue.Value == operation.Trim() || typeValue.Value == "All"));

            if (!checkAccess)
            {
               
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Error" }));
                //filterContext.Controller.ViewData
            } //new HttpUnauthorizedResult();
        }
    }
}