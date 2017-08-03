namespace YoloDev.AspNetCore.Assets.Options
{
  public class PathOptions : IPathOptions
  {
    public static PathOptions Default() => new PathOptions(
      useDevelopmentAssets: true,
      devServer: null);

    private DelegatedValue<bool> _useDevelopmentAssets;
    private DelegatedValue<string> _devServer;

    public bool UseDevelopmentAssets
    {
      get => _useDevelopmentAssets;
      set => _useDevelopmentAssets = value;
    }

    public string DevServer
    {
      get => _devServer;
      set => _devServer = value;
    }

    public IPathOptions Set(
      Optional<bool> useDevelopmentAssets = default(Optional<bool>),
      Optional<string> devServer = default(Optional<string>))
    {
      useDevelopmentAssets.Set(ref _useDevelopmentAssets);
      devServer.Set(ref _devServer);

      return this;
    }

    private PathOptions(
      bool useDevelopmentAssets,
      string devServer)
    {
      _useDevelopmentAssets = useDevelopmentAssets;
      _devServer = devServer;
    }

    internal PathOptions(PathOptions parent)
    {
      _useDevelopmentAssets = new DelegatedValue<bool>(parent._useDevelopmentAssets);
      _devServer = new DelegatedValue<string>(parent._devServer);
    }
  }
}
