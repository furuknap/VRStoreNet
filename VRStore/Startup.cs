using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VRStore.Startup))]
namespace VRStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
