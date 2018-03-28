using Microsoft.AspNetCore.Http;
using System.Collections.Immutable;
using System.Linq;
using YoloDev.AspNetCore.Assets.Options;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetOptions : IPathOptions
  {
    private readonly PathOptions _base;
    private ImmutableDictionary<PathString, PathOptions> _options =
      ImmutableDictionary.Create<PathString, PathOptions>();

    public bool UseDevelopmentAssets
    {
      get => _base.UseDevelopmentAssets;
      set => _base.UseDevelopmentAssets = value;
    }

    public string DevServer
    {
      get => _base.DevServer;
      set => _base.DevServer = value;
    }

    public AssetOptions() : this(PathOptions.Default(PathString.Empty)) { }

    internal AssetOptions(PathOptions baseOptions)
    {
      _base = baseOptions;
    }

    private PathOptions CreatePathOptions(PathString path)
    {
      var segments = path.Value.Substring(1).Split('/');
      var dir = path.Value.EndsWith("/");
      var parentPath = new PathString("/" + string.Join("/", segments.Take(segments.Length - 1)) + (dir ? "/" : ""));
      var parent = ForPathInternal(parentPath);
      return new PathOptions(path, parent);
    }

    public IPathOptions ForPath(PathString path) => ForPathInternal(path);

    internal PathOptions ForPathInternal(PathString path)
    {
      if (!path.HasValue || path == "/")
      {
        return _base;
      }

      return ImmutableInterlocked.GetOrAdd(ref _options, path, CreatePathOptions);
    }

    public IPathOptions Set(
      Optional<bool> useDevelopmentAssets = default(Optional<bool>),
      Optional<string> devServer = default(Optional<string>))
    {
      _base.Set(useDevelopmentAssets, devServer);

      return this;
    }
  }
}
