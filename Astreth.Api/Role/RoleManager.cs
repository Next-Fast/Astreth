using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Astreth.Api.Role;

public class RoleManager(IServiceProvider provider)
{
    private List<IRole> AllRoles { get; set; } = [];
    private List<IRoleCotroller> Controllers { get; set; } = [];

    public T? GetRole<T>() where T : class, IRole => AllRoles.FirstOrDefault(n => n is T) as T;
    public T? GetCotroller<T>(IModPlayer player) where T : class, IRoleCotroller => Controllers.FirstOrDefault(n => n.Player == player) as T;

    public RoleManager LoadFromService()
    {
        foreach (var role in provider.GetServices<IRole>())
        {
            AllRoles.Add(role);
        }
        
        return this;
    }

    public RoleManager LoadRolesFormAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes().Where(n => n.IsAssignableFrom(typeof(IRole))))
        {
            try
            {
                if (ActivatorUtilities.CreateInstance(provider, type) is IRole instance)
                {
                    AllRoles.Add(instance);
                }
            }
            catch
            {
                // ignored
            }
        }
        
        return this;
    }

    public RoleManager Register(IRole role)
    {
        AllRoles.Add(role);
        return this;
    }
}