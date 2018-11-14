using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TussoTechWebsite.Startup))]
namespace TussoTechWebsite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            try
            {
                ConfigureAuth(app);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
