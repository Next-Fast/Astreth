namespace Astreth.Api.Option;

public class BoolOption : IOption
{
    public BoolOption() { }

    public BoolOption(string optionId, bool value)
    {
        OptionId = optionId;
        Value = value;
    }
    
    public string Serialize()
    {
        return $"${Type}:{OptionId}:{Value}";
    }

    public bool Deserialize(string optionId, string content)
    {
        if (!bool.TryParse(content, out var value))
        {
            return false;
        }
        
        OptionId = optionId;
        Value = value;
        
        return true;
    }

    public void ChangeValue(bool value) => Value = value;    

    public string Type => "bool";
    public bool Value { get; private set; }
    public string OptionId { get; private set; } = "Unknown";
}