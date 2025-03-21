namespace Astreth.Api.Attributes;

internal interface IOptionBind
{
    string OptionId { get; }
    string type { get; } 
};

[AttributeUsage(AttributeTargets.Field)]
public class OptionBindAttribute<T>(string optionId) : Attribute, IOptionBind
{
    public string OptionId { get; } = optionId;
    
    public string type => nameof(T);

    public Action<T>? SetValue;
}