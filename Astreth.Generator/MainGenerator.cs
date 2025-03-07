using Microsoft.CodeAnalysis;

namespace Astreth.Generator;

[Generator(LanguageNames.CSharp)]
public sealed class MainGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
    }
}