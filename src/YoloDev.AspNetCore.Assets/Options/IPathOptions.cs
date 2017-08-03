namespace YoloDev.AspNetCore.Assets.Options
{
  public interface IPathOptions
  {
    bool UseDevelopmentAssets { get; set; }
    //string DevServer { get; set; }

    IPathOptions Set(
      Optional<bool> useDevelopmentAssets = default(Optional<bool>)//,
      /*Optional<string> devServer = default(Optional<string>)*/);
  }
}
