using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetManifestStreamProvider : IAssetManifestStreamProvider
  {
    private const string ManifestName = "manifest.json";

    private readonly IFileProvider _files;

    public AssetManifestStreamProvider(IHostingEnvironment env)
      : this(env?.WebRootFileProvider)
    {
    }

    public AssetManifestStreamProvider(IFileProvider files)
    {
      if (files == null)
      {
        throw new ArgumentNullException(nameof(files));
      }

      _files = files;
    }

    public Task<AssetFileInfo> FindManifest(PathString pathString)
    {
      var path = pathString.Value.Substring(1); // remove first slash
      while (true)
      {
        var dir = Path.GetDirectoryName(path);
        if (string.IsNullOrEmpty(dir))
          return Task.FromResult<AssetFileInfo>(null);

        var manifestPath = Path.Combine(dir, ManifestName);
        var manifestFile = _files.GetFileInfo(manifestPath);
        if (manifestFile.Exists)
        {
          return Task.FromResult(new AssetFileInfo(
            new PathString("/" + dir.Replace('\\', '/') + "/" + manifestFile.Name),
            manifestFile
          ));
        }

        path = dir;
      }
    }
  }
}
