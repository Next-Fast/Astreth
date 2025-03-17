using Astreth.Api.Option;
using Astreth.Api.Role;
using Microsoft.Extensions.DependencyInjection;
using NextBepLoader.Core;
using NextBepLoader.Core.LoaderInterface;

namespace Astreth;

public class AstrethStartup : IStartup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<RoleManager>();
        services.AddSingleton<OptionManager>();
    }
}