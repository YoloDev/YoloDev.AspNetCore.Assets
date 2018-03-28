using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  /// <summary>
  /// Service for getting assets.
  /// </summary>
  public interface IAssetService
  {
    /// <summary>
    /// Get runtime asset path for given asset.
    /// </summary>
    /// <param name="path">The original asset path.</param>
    /// <returns>The runtime asset path.</returns>
    Task<string> GetAsset(PathString path, HttpContext context);
  }
}
