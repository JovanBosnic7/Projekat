using System;
using System.Threading.Tasks;
using AspNet.DAL.EF.UOW.Security;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BexMVC.OwinStartup))]

namespace BexMVC
{
    public class OwinStartup
    {
        public void Configuration(IAppBuilder app)
        {
            SecurityUow.ConfigureAuth(app);
        }
    }
}
