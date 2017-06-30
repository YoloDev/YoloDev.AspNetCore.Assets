using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetService : IAssetService
  {
    private readonly IOptionsSnapshot<AssetOptions> _options;
    private readonly IAssetManifestCache _manifests;

    public AssetService(
      IOptionsSnapshot<AssetOptions> options,
      IAssetManifestCache manifests)
    {
      _options = options;
      _manifests = manifests;
    }

    public async Task<PathString> GetAsset(PathString path, HttpContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      if (!path.HasValue)
      {
        throw new ArgumentException("Path cannot be empty", nameof(path));
      }

      if (_options.Value.UseDevelopmentAssets)
      {
        return GetBasePath(context).Add(path);
      }

      var manifest = await _manifests.GetManifest(path);
      if (manifest == null)
      {
        return GetBasePath(context).Add(path);
      }

      // TODO: Better handling of relative paths
      if (!path.StartsWithSegments(PathUtils.Dir(manifest.Path), out PathString start, out PathString rel))
        throw new InvalidOperationException("Manifest was not relative to asset");

      var rewrite = manifest.Resolve(rel);
      return GetBasePath(context).Add(start).Add(rewrite);
    }

    private static PathString GetBasePath(HttpContext context) =>
      context.Request.PathBase;
  }
}
