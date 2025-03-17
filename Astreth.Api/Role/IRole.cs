namespace Astreth.Api.Role;

public interface IRole
{
    public IRoleCotroller CreateCotroller(IModPlayer player, IRoleArgument argument);
}

public interface IRoleArgument;