namespace Astreth.Api.Option;

public interface IOption : ISerializable<IOption>
{
    public string OptionId { get; }
}