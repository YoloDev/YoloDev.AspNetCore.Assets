using Microsoft.AspNetCore.Http;

namespace YoloDev.AspNetCore.Assets.Options
{
  internal class PathOptions : IPathOptions
  {
    public static PathOptions Default(PathString path) => new PathOptions(
      path: path,
      useDevelopmentAssets: false,
      devServer: null);

    private readonly PathString _path;
    private DelegatedValue<bool> _useDevelopmentAssets;
    private DelegatedValue<DevServerOptions> _devServer;

    public bool UseDevelopmentAssets
    {
      get => _useDevelopmentAssets;
      set => _useDevelopmentAssets = value;
    }

    public string DevServer
    {
      get => _devServer.Value?.Url;
      set => _devServer = new DevServerOptions(_path, value);
    }

    internal DevServerOptions DevServerOptions => _devServer;

    public IPathOptions Set(
      Optional<bool> useDevelopmentAssets = default(Optional<bool>),
      Optional<string> devServer = default(Optional<string>))
    {
      useDevelopmentAssets.Set(ref _useDevelopmentAssets);
      devServer.Set(ref _devServer, url => new DevServerOptions(_path, url));

      return this;
    }

    private PathOptions(
      PathString path,
      bool useDevelopmentAssets,
      DevServerOptions devServer)
    {
      _path = path;
      _useDevelopmentAssets = useDevelopmentAssets;
      _devServer = devServer;
    }

    internal PathOptions(PathString path, PathOptions parent)
    {
      _path = path;
      _useDevelopmentAssets = new DelegatedValue<bool>(parent._useDevelopmentAssets);
      _devServer = new DelegatedValue<DevServerOptions>(parent._devServer);
    }
  }
}
