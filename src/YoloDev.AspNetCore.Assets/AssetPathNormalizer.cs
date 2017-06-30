using System;
using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetPathNormalizer : IAssetPathNormalizer
  {
    public PathString Normalize(PathString path) =>
      new PathString(path.Value?.ToLowerInvariant());
  }
}
