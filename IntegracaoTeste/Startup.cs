using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IntegracaoTeste.Startup))]
namespace IntegracaoTeste
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
