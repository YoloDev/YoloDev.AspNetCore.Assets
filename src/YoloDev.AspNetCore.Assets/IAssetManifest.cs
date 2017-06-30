using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  /// <summary>
  /// Represents an asset manifest that contain mappings from
  /// asset names to output paths.
  /// </summary>
  public interface IAssetManifest
  {
    /// <summary>
    /// Gets the path of the <see cref="AssetManifest" />.
    /// </summary>
    /// <returns>The path of the manifest.</returns>
    PathString Path { get; }

    /// <summary>
    /// Resolve a asset relative to the manifest file.
    /// If the asset is not found, the original name is
    /// returned.
    /// </summary>
    /// <param name="path">The asset path, relative to the manifest.</param>
    /// <returns>The new asset path, relative to the manifest.</returns>
    PathString Resolve(PathString path);
  }
}
