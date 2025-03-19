using NextBepLoader.Core;

namespace Astreth.Api.Utils;

public static class Paths
{
    private static string Combine(this string a, string b) => Path.Combine(a, b);

    private static string CheckCreate(this string path)
    {
        if (Directory.Exists(path))
        {
            return path;
        }
        
        Directory.CreateDirectory(path);
        return path;
    }

    public static string ResourcePath => NextBepLoader.Core.Paths.LoaderRootPath.Combine("Resource").CheckCreate();
    
    public static string AssetsPath => ResourcePath.Combine("Assets").CheckCreate();
    
    public static string TranslationPath => ResourcePath.Combine("Translation").CheckCreate();
    
    public static string AudioPath => ResourcePath.Combine("Audio").CheckCreate();
    
    public static string SpritePath => ResourcePath.Combine("Sprite").CheckCreate();
}