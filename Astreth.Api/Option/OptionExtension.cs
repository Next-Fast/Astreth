using System.Diagnostics.CodeAnalysis;

namespace Astreth.Api.Option;

public static class OptionExtension
{
    /*internal static bool TryParseNumberOption<T>(this string content, Func<string, (bool, T)> Parse, [MaybeNullWhen(false)]out List<T> result)
    {
        var list = new List<T>();
        var values = content.Split('-');
        foreach (var value in values)
        {
            var (success, parsedValue) = Parse(value);
            if (success)
            {
                list.Add(parsedValue);
                continue;
            }
            
            result = null;
            return false;
        }

        result = list;
        return true;
    }*/
}