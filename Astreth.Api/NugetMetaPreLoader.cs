using System.IO.Compression;
using System.Text.Json;
using System.Xml;
using Microsoft.Extensions.Logging;
using NextBepLoader.Core;
using NextBepLoader.Core.PreLoader;

namespace Astreth.Api;

public class NugetMetaPreLoader(ILogger<NugetMetaPreLoader> logger, HttpClient client) : BasePreLoader
{
    public override void PreLoad(PreLoadEventArg arg) => Load().Start();

    private async Task Load()
    {
        foreach (var path in Directory.GetFiles(Paths.DependencyDirectory, "*.nuget.json"))
        {
            await using var stream = File.OpenRead(path);
            using var reader = new StreamReader(stream);
            var infos = JsonSerializer.Deserialize<List<DependencyInfo>>(await reader.ReadToEndAsync());
            if (infos == null) continue;
            foreach (var info in infos)
            {
                var file = Path.Combine(Paths.DependencyDirectory, info.Id + ".dll");
                if (File.Exists(file)) continue;
                await using var dllStream = Download(info.Id, info.Version, info.Framework);
                if (dllStream == Stream.Null) continue;
                await using var fileStream = File.OpenWrite(file);
                await dllStream.CopyToAsync(fileStream);
            }
        }
    }
    
    private record DependencyInfo(string Id, string Version, string Framework);

    private Stream Download(string name, string version, string framework = "net9.0")
    {
        var downloader = new NuGetDownloader(client, name, version);
        var get = new NugetZipGet(downloader);
        var frameworks = get.GetFrameworks();
        var getFramework = GetFramework(frameworks, framework);
        return getFramework == string.Empty ? Stream.Null : get.GetAssemblyStream(GetPathFramework(getFramework));
    }

    private static string GetPathFramework(string framework)
    {
        var path = framework.ToLower();
        return path[0] == '.' ? path.Remove(0) : path;
    }

    private static string GetFramework(string[] frameworks, string framework)
    {
        if (frameworks.Contains(framework)) return framework;
        var ver = framework.Substring(framework.LastIndexOf('.') - 1, 3);
        var versionInt = ToVersionInt(framework);
        var frameworkName = framework.Replace(ver, string.Empty);
        var fs = frameworks.Where(n => n.StartsWith(frameworkName)).ToList();
        if (fs.Count != 0)
        {
            foreach (var sf in from sf in fs let sVersionInt = ToVersionInt(sf) where sVersionInt <= versionInt select sf)
            {
                return sf;
            }
        }

        if (!framework.StartsWith("net")) return string.Empty;
        var Standards = frameworks.Where(n => n.StartsWith(".NETStandard")).ToList();
        if (Standards.Count == 0) return string.Empty;
        Standards.Sort((x, y) => ToVersionInt(x).CompareTo(ToVersionInt(y)));
        return Standards.Last();

        int ToVersionInt(string versionString)
        {
            return int.Parse(versionString.Substring(versionString.LastIndexOf('.') - 1, 3).Replace(".", string.Empty));
        }
    }

    private class NuGetDownloader(HttpClient client, string Id, string version)
    {
        private const string ApiRootUrl = "https://api.nuget.org/v3-flatcontainer";
        private string LowerId => Id.ToLowerInvariant();
        private string LowerVersion => version.ToLowerInvariant();

        public async Task<Stream> Download()
        {
            var url = $"{ApiRootUrl}/{LowerId}/{LowerVersion}/{LowerId}.{LowerVersion}.nupkg";
            return await client.GetStreamAsync(url);
        }
    }

    private class NugetZipGet(NuGetDownloader downloader)
    {
        private readonly ZipArchive ZipArchive = new(downloader.Download().Result);

        public string[] GetFrameworks()
        {
            var document = new XmlDocument();
            document.LoadXml(GetDocumentText());
            var groups = document
                .DocumentElement?
                .FirstChild?
                .FindOneXML("dependencies")
                .FindXML("group");
        
            return groups == null
                ? []
                : (from XmlElement @group in groups select @group.GetAttribute("targetFramework"))
                .ToArray();
        
        }

        private string GetDocumentText()
        {
            return new StreamReader(ZipArchive.Entries
                .First(n => n.FullName.EndsWith(".nuspec"))
                .Open()).ReadToEnd();
        }
    
        public Stream GetAssemblyStream(string Framework)
        {
            return ZipArchive.Entries
                .Where(n => n.FullName.EndsWith(".dll"))
                .First(n => n.FullName.Contains(Framework))
                .Open();
        }
    }
}