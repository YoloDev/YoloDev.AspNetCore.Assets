using System;
using System.IO;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Xunit;

namespace YoloDev.AspNetCore.Assets.Test
{
  public class AssetManifestCacheTest
  {
    [Fact]
    public void ConstructorThrowsIfStreamProviderIsNull()
    {
      Assert.Throws<ArgumentNullException>("provider", () => new AssetManifestCache(null, A.Fake<IAssetManifestReader>()));
    }

    [Fact]
    public void ConstructorThrowsIfReaderIsNull()
    {
      Assert.Throws<ArgumentNullException>("reader", () => new AssetManifestCache(A.Fake<IAssetManifestStreamProvider>(), null));
    }

    [Fact]
    public async Task CachesTheManifests()
    {
      var path1 = new PathString("/assets/1.js");
      var path2 = new PathString("/assets/2.js");
      var manifestPath = new PathString("/assets/manifest.json");
      var file = new AssetFileInfo(
        manifestPath,
        A.Fake<IFileInfo>()
      );

      var provider = A.Fake<IAssetManifestStreamProvider>();
      A.CallTo(() => provider.FindManifest(A<PathString>.Ignored))
        .Returns(Task.FromResult(file));

      var reader = A.Fake<IAssetManifestReader>();

      var sut = new AssetManifestCache(provider, reader);
      await sut.GetManifest(path1);
      await sut.GetManifest(path2);

      A.CallTo(() => provider.FindManifest(path1)).MustHaveHappened();
      A.CallTo(() => provider.FindManifest(path2)).MustHaveHappened();

      A.CallTo(() => reader.Read(manifestPath, A<Stream>.Ignored))
        .MustHaveHappened(Repeated.Exactly.Once);
    }
  }
}
