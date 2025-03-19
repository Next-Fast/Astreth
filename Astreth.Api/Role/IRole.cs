using Astreth.Api.Option;

namespace Astreth.Api.Role;

public interface IRole
{
    public IRoleCotroller CreateCotroller(IModPlayer player, IRoleArgument argument);

    public IRole CreateOption(IOptionCreator creator);
}

public interface IRoleArgument;