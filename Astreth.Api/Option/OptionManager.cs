using System.Diagnostics.CodeAnalysis;
using Il2CppSystem.Text;
using Microsoft.Extensions.Logging;

namespace Astreth.Api.Option;

public class OptionManager(ILogger<OptionManager> logger)
{
    private List<IOption> AllOptions { get; set; } = [];

    public OptionManager SaveTo(string path)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Astreth@Options{");
        
        foreach (var option in AllOptions)
        {
            var content = option.Serialize();
            builder.AppendLine($"${option.Type}:{option.OptionId}:{content};");
        }

        builder.AppendLine("}");
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
        
        var optionsText = text.Substring(text.IndexOf('{'), text.Length - 1);
        var options = optionsText.Split(';');
        foreach (var option in options)
        {
            var part = option
                .Trim()
                .Trim('\n')
                .Split(':');
            
            if (!part[0].StartsWith('$'))
                continue;
            
            var type = part[0].Remove(0);
            var optionId = part[1];
            var content = part[2];

            var get = GetOption(optionId);
            if (get == null) continue;
            
            if (get.Type != type)
            {
                continue;
            }
                
            if (!get.Deserialize(content))
            {
                logger.LogWarning("Deserialization failed for option {OptionId}", optionId);
            }
        }
        
        return this;
    }

    /*private static bool TryCreateOption(string type, string optionId, string content,[MaybeNullWhen(false)]out IOption option)
    {
        option = type switch
        {
            "bool" => new BoolOption(optionId),
            "float" => new FloatOption(optionId),
            _ => null
        };

        return option != null && option.Deserialize(content);
    }*/
    
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
}

public interface IOptionCreator
{
    public IOptionCreator AddBoolOption(string optionId, bool value);
    public IOptionCreator AddNumberOption<T>(string optionId, T value, T Min, T Max, T Step) where T : unmanaged;
    
    public IOptionCreator AddMultipleStringOption(string optionId, string[] options, int[] selectedIndex);
    
    public IOptionCreator AddSingleStringOption(string optionId, string[] options, int defaultIndex = 0);
    
    public IOptionCreator AddEnumOption<T>(string optionId, T value) where T : struct, Enum;
    
    public void RegisterToManager(OptionManager manager);
}

public class BaseOptionCreator : IOptionCreator
{
    static BaseOptionCreator()
    {
        INumberOptionCreator<float>.Instance = new FloatOption.FloatOptionCreator();
        INumberOptionCreator<int>.Instance = new IntOption.IntOptionCreator();
    }
    
    
    private List<IOption> Options { get; } = [];
    public virtual IOptionCreator AddBoolOption(string optionId, bool value)
    {
        Options.Add(new BoolOption(optionId, value));
        return this;
    }

    public virtual IOptionCreator AddNumberOption<T>(string optionId, T value, T min, T max, T step) where T : unmanaged
    {
        var create = INumberOptionCreator<T>.Instance?.Create(optionId, value, min, max, step);
        if (create == null)
        {
            Log?.LogWarning($"No number option creator found for type {nameof(T)}");
            return this;
        }

        Options.Add(create);
        return this;
    }

    public IOptionCreator AddMultipleStringOption(string optionId, string[] options, int[] selectedIndex)
    {
        throw new NotImplementedException();
    }

    public IOptionCreator AddSingleStringOption(string optionId, string[] options, int defaultIndex = 0)
    {
        throw new NotImplementedException();
    }

    public virtual IOptionCreator AddEnumOption<T>(string optionId, T value) where T : struct, Enum
    {
        Options.Add(new EnumOption<T>(optionId, value));
        return this;
    }

    public void RegisterToManager(OptionManager manager)
    {
        foreach (var option in Options)
        {
            manager.RegisterOption(option);
        }
    }
}