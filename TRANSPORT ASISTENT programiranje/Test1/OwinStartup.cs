using System;
using System.Threading.Tasks;
using AspNet.DAL.EF.UOW.Security;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DDtrafic.OwinStartup))]

namespace DDtrafic
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            SecurityUow.ConfigureAuth(app);
        }
    }
}
