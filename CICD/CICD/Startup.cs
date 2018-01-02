using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CICD.Startup))]
namespace CICD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
