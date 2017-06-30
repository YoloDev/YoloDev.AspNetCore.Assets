using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  /// <summary>
  /// A cache of <see cref="IAssetManifest" />.
  /// </summary>
  public interface IAssetManifestCache
  {
    /// <summary>
    /// Get a manifest given an asset path.
    /// </summary>
    /// <param name="assetPath">The asset path.</param>
    /// <returns>The asset manifest, or <c>null</c> if not found.</returns>
    Task<IAssetManifest> GetManifest(PathString path);
  }
}
