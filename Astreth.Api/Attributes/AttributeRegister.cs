using System.Reflection;

namespace Astreth.Api.Attributes;

public class AttributeRegister
{
    public AttributeRegister FindFromAssembly(Assembly assembly)
    {
        return this;
    }

    public AttributeRegister Register<T>(Action<Type, T> OnRegister) where T : Attribute
    {
        return this;
    }
}