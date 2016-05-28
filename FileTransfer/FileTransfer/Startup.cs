using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FileTransfer.Startup))]
namespace FileTransfer
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
