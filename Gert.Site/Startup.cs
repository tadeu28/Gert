using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gert.Site.Startup))]
namespace Gert.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
