using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppleShop.Startup))]
namespace AppleShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
