using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EnjoyFootballWebb.Startup))]
namespace EnjoyFootballWebb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
