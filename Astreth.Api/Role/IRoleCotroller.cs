namespace Astreth.Api.Role;

public interface IRoleCotroller : IDisposable
{
    IModPlayer Player { get; }
}