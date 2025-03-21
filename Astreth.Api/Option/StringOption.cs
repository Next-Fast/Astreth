namespace Astreth.Api.Option;

public class StringOption(string optionId) : IOption
{
    public string Serialize()
    {
        var header = $"{IsMultiple}^";
        var content = IsMultiple ? string.Join('|', SelectIndex.Select(n => $"{n}")) : $"{SelectIndex[0]}";
        return header + content;
    }

    public bool Deserialize(string content)
    {
        var parts = content.Split('^');
        if (parts.Length != 2) return false;
        if (!bool.TryParse(parts[0], out var result))
        {
            return false;
        }
        
        IsMultiple = result;
        var part2 = IsMultiple ? parts[1].Split('|') : [parts[1]];
        var newSelect = new List<int>();
        for (var index = 0; index < part2.Length - 1; index++)
        {
            if (!int.TryParse(part2[index], out var intResult))
            {
                return false;
            }
            
            newSelect.Add(intResult);
        }
        
        SelectIndex = newSelect;
        return true;
    }

    public string[] Options { get; private set; }
    public List<int> SelectIndex { get; private set; }
    public bool IsMultiple { get; private set; }
    public string OptionId { get; } = optionId;
    public string Type => "string";
}