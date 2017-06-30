using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  /// <summary>
  /// Normalizes paths to assets.
  /// </summary>
  public interface IAssetPathNormalizer
  {
    /// <summary>
    /// Normalize asset path.
    /// </summary>
    /// <param name="path">Asset path.</param>
    /// <returns>Normalized path.</returns>
    PathString Normalize(PathString path);
  }
}
