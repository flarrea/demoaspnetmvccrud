using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LoginEmpleado.CRUD.Startup))]
namespace LoginEmpleado.CRUD
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
