using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(uetquizing.Startup))]
namespace uetquizing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
