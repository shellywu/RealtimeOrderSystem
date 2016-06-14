using Microsoft.Owin;
using OrderSystem.Core.RealTimeConn.Core;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrderSystem.Core.Startup))]
namespace OrderSystem.Core
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR<OrderConnection>("/OrderInfo");
            app.MapSignalR();
        }
    }
}
