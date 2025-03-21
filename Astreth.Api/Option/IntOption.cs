namespace Astreth.Api.Option;

public class IntOption(string optionId) : INumberOption<int>
{
    internal class IntOptionCreator : INumberOptionCreator<int>
    {
        public INumberOption<int>? Create(string optionId, int value, int min, int max, int step)
        {
            return new IntOption(optionId, min, max, step, value);
        }
    }
    
    public IntOption(string optionId, int min, int max, int step, int value) : this(optionId)
    {
        Min = min;
        Max = max;
        Step = step;
        Value = value;
    }
    
    public int Min { get; private set; }
    public int Max { get; private set; }
    public int Step { get; private set; }
    public int Value { get; private set; }
    
    public string OptionId { get; } = optionId;
    
    public string Type => "int";
    
    
    public static explicit operator int(IntOption option) => option.Value;

    public bool Deserialize(string content)
    {
        if (!int.TryParse(content, out var result))
        {
            return false;
        }

        if (result < Min || result > Max)
        {
            return false;
        }
        
        Value = result;
        return true;
    }
    
    /*public bool Deserialize(string content)
    {
        if (!content.TryParseNumberOption(
                text => 
                    int.TryParse(text, out var result) ? (true, result) : (false, 0), 
                out var values)
           )
        {
            return false;
        }

        if (values.Count > 4)
        {
            return false;
        }

        Min = values[0];
        Max = values[1];
        Step = values[2];
        Value = values[3];
        return true;
    }*/
}