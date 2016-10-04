using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebMessageSubmit.Startup))]
namespace WebMessageSubmit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
