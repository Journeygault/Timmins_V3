using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TiminsHospitalProjectV3.Startup))]
namespace TiminsHospitalProjectV3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
