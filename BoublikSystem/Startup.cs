using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BoublikSystem.Startup))]
namespace BoublikSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
