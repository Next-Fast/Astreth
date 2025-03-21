namespace Astreth.Api.Option;

public class FloatOption(string optionId) : INumberOption<float>
{
    internal class FloatOptionCreator : INumberOptionCreator<float>
    {
        public INumberOption<float> Create(string optionId, float value, float min, float max, float step)
        {
            return new FloatOption(optionId, min, max, step, value);
        }
    }
    
    public FloatOption(string optionId, float min, float max, float step, float value) : this(optionId)
    {
        Min = min;
        Max = max;
        Step = step;
        Value = value;
    }
    
    
    public bool Deserialize(string content)
    {
        if (!float.TryParse(content, out var result))
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
                float.TryParse(text, out var result) ? (true, result) : (false, 0f), 
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
    
    public float Min { get; private set; }
    public float Max { get; private set; }
    public float Step { get; private set; }
    public float Value { get; private set; }

    public string OptionId { get; } = optionId;
    public string Type => "float";
    
    public static explicit operator float(FloatOption option) => option.Value;
}