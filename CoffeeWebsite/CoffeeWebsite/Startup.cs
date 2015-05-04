using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(CoffeeWebsite.Startup))]
namespace CoffeeWebsite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }


    }
}
