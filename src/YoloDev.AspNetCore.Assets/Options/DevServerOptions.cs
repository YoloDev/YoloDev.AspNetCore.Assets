using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets.Options
{
  internal class DevServerOptions
  {
    private readonly PathString _basePath;
    private readonly string _url;

    public DevServerOptions(PathString basePath, string url)
    {
      _basePath = basePath;
      _url = url;
    }

    public PathString BasePath => _basePath;
    public string Url => _url;
  }
}
