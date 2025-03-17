namespace Astreth.Api.Option;

public interface ISerializable<out T> where T : ISerializable<T>
{
    public string Serialize();
    
    public bool Deserialize(string optionId, string content);
}