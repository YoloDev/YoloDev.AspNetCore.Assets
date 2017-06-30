using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using YoloDev.AspNetCore.Assets;

namespace Microsoft.Extensions.DependencyInjection
{
  public static class ServiceCollectionExtensions
  {
    public static IServiceCollection AddAssets(this IServiceCollection services)
    {
      services.TryAddSingleton<IAssetManifestCache, AssetManifestCache>();
      services.TryAddSingleton<IAssetManifestReader, AssetManifestReader>();
      services.TryAddSingleton<IAssetManifestStreamProvider, AssetManifestStreamProvider>();
      services.TryAddSingleton<IAssetPathNormalizer, AssetPathNormalizer>();
      services.TryAddSingleton<IAssetService, AssetService>();
      services.TryAddTransient<IConfigureOptions<AssetOptions>, AssetOptionsSetup>();

      return services;
    }

    public static IServiceCollection AddAssets(this IServiceCollection services, Action<AssetOptions> configure)
    {
      AddAssets(services);
      services.Configure(configure);

      return services;
    }
  }
}
