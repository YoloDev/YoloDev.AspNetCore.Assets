using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;

namespace YoloDev.AspNetCore.Assets
{
  public class AssetOptionsSetup : IConfigureOptions<AssetOptions>
  {
    private readonly IHostingEnvironment _env;

    public AssetOptionsSetup(IHostingEnvironment env)
    {
      _env = env;
    }

    public void Configure(AssetOptions options)
    {
      options.UseDevelopmentAssets = _env.IsDevelopment();
    }
  }
}
