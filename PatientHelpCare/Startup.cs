using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PatientHelpCare.Startup))]
namespace PatientHelpCare
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
