﻿using System.Drawing;
using Microsoft.Extensions.Logging;
using NextBepLoader.Core.Contract;
using NextBepLoader.Core.Contract.Attributes;
using Console = Colorful.Console;

namespace Astreth;

// ReSharper disable once UseSymbolAlias
[PluginMetadata(LoaderPlatformType.Desktop, "Astreth.Fast.Next")]
public class AstrethPlugin(ILogger<AstrethPlugin> logger) : BasePlugin
{
    public override void Load()
    {
        Console.WriteAscii("Next Fast", ColorTranslator.FromHtml("#459bf7"));
        Console.WriteAscii("Astreth", Color.CornflowerBlue);
        
        
    }
}