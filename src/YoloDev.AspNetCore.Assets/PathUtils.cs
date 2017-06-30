using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets
{
  internal class PathUtils
  {
    internal static PathString Dir(PathString path)
    {
      if (!path.HasValue)
      {
        throw new ArgumentException("Path must have value", nameof(path));
      }

      var str = path.Value;
      if (str.Equals("/"))
      {
        return null;
      }

      if (str.EndsWith("/"))
      {
        str = str.Substring(0, str.Length - 1);
      }

      var lastSlash = str.LastIndexOf('/');
      if (lastSlash < 1)
      {
        return new PathString("/");
      }

      return new PathString(str.Substring(0, lastSlash));
    }
  }
}
