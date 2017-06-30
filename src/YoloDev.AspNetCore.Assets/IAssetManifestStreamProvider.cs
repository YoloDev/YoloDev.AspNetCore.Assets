using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace YoloDev.AspNetCore.Assets
{
  public interface IAssetManifestStreamProvider
  {
    Task<AssetFileInfo> FindManifest(PathString path);
  }
}
