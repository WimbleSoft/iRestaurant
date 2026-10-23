using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(iRestaurant.Startup))]
namespace iRestaurant
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
    
}
