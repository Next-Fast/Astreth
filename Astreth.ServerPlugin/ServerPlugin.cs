using Impostor.Api.Extension;
using Impostor.Api.Plugins;
using Microsoft.Extensions.DependencyInjection;

namespace Astreth.ServerPlugin;

[ImpostorPlugin("Astreth.ServerPlugin")]
public class ServerPlugin : IPlugin, IHttpPluginStartup
{
    public bool AssemblyPart => true;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ModInfoManager>();
    }
}