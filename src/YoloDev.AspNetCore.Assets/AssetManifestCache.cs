using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetManifestCache : IAssetManifestCache
  {
    private readonly IAssetManifestStreamProvider _provider;
    private readonly IAssetManifestReader _reader;
    private ImmutableDictionary<PathString, IAssetManifest> _cache =
      ImmutableDictionary.Create<PathString, IAssetManifest>();

    public AssetManifestCache(
      IAssetManifestStreamProvider provider,
      IAssetManifestReader reader)
    {
      _provider = provider ?? throw new ArgumentNullException(nameof(provider));
      _reader = reader ?? throw new ArgumentNullException(nameof(reader));
    }

    public async Task<IAssetManifest> GetManifest(PathString assetPath)
    {
      var file = await _provider.FindManifest(assetPath);
      if (file == null)
        return null;

      if (_cache.TryGetValue(file.Path, out IAssetManifest cached))
        return cached;

      using (var stream = file.CreateReadStream())
      {
        var manifest = await _reader.Read(file.Path, stream);
        manifest = ImmutableInterlocked.GetOrAdd(ref _cache, file.Path, manifest);
        return manifest;
      }
    }
  }
}
