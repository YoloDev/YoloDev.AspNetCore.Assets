using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace YoloDev.AspNetCore.Assets
{
  public sealed class AssetFileInfo
  {
    private readonly PathString _path;
    private readonly IFileInfo _file;

    public AssetFileInfo(PathString path, IFileInfo file)
    {
      if (path == null)
      {
        throw new ArgumentNullException(nameof(path));
      }

      if (file == null)
      {
        throw new ArgumentNullException(nameof(file));
      }

      _path = path;
      _file = file;
    }

    public PathString Path => _path;
    public Stream CreateReadStream() => _file.CreateReadStream();
  }
}
