namespace Astreth.Api.Option;

public class BoolOption(string optionId) : IOption
{
    public BoolOption(string optionId, bool value) : this(optionId)
    {
        OptionId = optionId;
    }

    public string Serialize() => Value.ToString();
    public bool Deserialize(string content)
    {
        if (!bool.TryParse(content, out var value))
        {
            return false;
        }

        Value = value;
        return true;
    }

    public void ChangeValue(bool value) => Value = value;    

    public string Type => "bool";
    public bool Value { get; private set; }
    public string OptionId { get; } = optionId;
    
    public static explicit operator bool(BoolOption option) => option.Value;
}