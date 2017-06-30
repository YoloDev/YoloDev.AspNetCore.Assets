using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetManifest : IAssetManifest
  {
    private readonly PathString _path;
    private readonly IImmutableDictionary<PathString, PathString> _assets;

    public AssetManifest(PathString path, IImmutableDictionary<PathString, PathString> assets)
    {
      if (path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      if (assets == null)
      {
        throw new ArgumentNullException(nameof(assets));
      }

      _path = path;
      _assets = assets;
    }

    /// <inheritdoc />
    public PathString Path => _path;

    /// <inheritdoc />
    public PathString Resolve(PathString path)
    {
      return _assets.GetValueOrDefault(path, path);
    }
  }
}
