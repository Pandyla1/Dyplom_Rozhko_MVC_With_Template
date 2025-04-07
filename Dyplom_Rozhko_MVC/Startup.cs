using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dyplom_Rozhko_MVC.Startup))]
namespace Dyplom_Rozhko_MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
