using UnityEngine;

namespace Astreth.Api.Role;

public class RoleTeam(Color color, string Id)
{
    public Color Color { get; } = color;
    public string Id { get; } = Id;
}