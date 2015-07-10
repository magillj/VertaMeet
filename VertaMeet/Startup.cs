using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VertaMeet.Startup))]
namespace VertaMeet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
