using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JazzClouldApp.Startup))]
namespace JazzClouldApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
