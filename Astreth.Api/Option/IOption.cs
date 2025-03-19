namespace Astreth.Api.Option;

public interface IOption : ISerializable<IOption>
{
    public string OptionId { get; }
    public string Type { get; }
}

public interface INumberOptionCreator<T> where T : unmanaged
{
    public static INumberOptionCreator<T>? Instance = null;
    public INumberOption<T>? Create(string optionId, T value, T min, T max, T step);
}

public interface INumberOption<out T> : IOption where T : unmanaged
{
    public T Min { get; }
    public T Max { get; }
    public T Step { get; }
    
    public T Value { get; }
}
