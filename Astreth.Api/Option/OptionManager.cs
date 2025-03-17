using Il2CppSystem.Text;
using Microsoft.Extensions.Logging;

namespace Astreth.Api.Option;

public class OptionManager(ILogger<OptionManager> logger)
{
    private List<IOption> AllOptions { get; set; } = [];

    public OptionManager SaveTo(string path)
    {
        var builder = new StringBuilder();
        builder.Append("Astreth@Options{");
        
        foreach (var option in AllOptions)
        {
            builder.AppendLine(option.Serialize() + ";");
        }

        builder.Append("}");
        File.WriteAllText(path, builder.ToString());
        return this;
    }


    public OptionManager LoadFrom(string path)
    {
        if (!File.Exists(path))
            return this;
        var text = File.ReadAllText(path);
        if (!text.StartsWith("Astreth@Options{") || !text.EndsWith('}'))
            return this;

        AllOptions = [];
        var optionsText = text.Substring(text.IndexOf('{'), text.Length - 1);
        var options = optionsText.Split(';');
        foreach (var option in options)
        {
            var part = option.Split(':');
            var type = part[0];
            if (!type.StartsWith('$'))
                continue;

            var create = createOptionFromType(type.Remove(0));
            if (create == null) 
                continue;
            
            var optionId = part[1];
            var value = part[2];
            if (create.Deserialize(optionId, value))
            {
                AllOptions.Add(create);
            }
        }
        
        return this;
    }


    public OptionManager RegisterOption(IOption option)
    {
        if (AllOptions.Any(n => n.OptionId == option.OptionId))
        {
            logger.LogWarning("Option with id {OptionId} already exists", option.OptionId);
            return this;
        }
        
        AllOptions.Add(option);
        return this;
    }

    public OptionManager UnregisterOption(string optionId)
    {
        AllOptions.RemoveAll(n => n.OptionId == optionId);
        return this;
    }

    public T? GetOption<T>(string optionId) where T : class, IOption
    {
        return GetOption(optionId) as T;
    }

    public IOption? GetOption(string optionId)
    {
        return AllOptions.FirstOrDefault(o => o.OptionId == optionId);
    }

    private static IOption? createOptionFromType(string type)
    {
        return type switch
        {
            "bool" => new BoolOption(),
            _ => null
        };
    }
}