using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  public interface IAssetManifestReader
  {
    Task<IAssetManifest> Read(PathString path, Stream stream);
  }
}
