namespace Astreth.Api.Option;

public class EnumOption<T>(string optionId) : IOption where T : struct, Enum
{
    public string Serialize()
    {
        return Value.ToString();
    }

    public bool Deserialize(string content)
    {
        if (!Enum.TryParse<T>(content, out var value))
        {
            return false;
        }
        
        Value = value;
        return true;
    }

    public EnumOption(string optionId, T value) : this(optionId)
    {
        Value = value;
    }

    public string[] Options = Enum.GetNames<T>();
    public T Value { get; private set; }

    public static implicit operator T(EnumOption<T> option) => option.Value;

    public bool isFlags => typeof(T).IsDefined(typeof(FlagsAttribute), false);
    public string OptionId { get; } = optionId;
    public string Type => "enum";
}