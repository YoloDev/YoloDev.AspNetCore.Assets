using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Xunit;
using YoloDev.AspNetCore.Assets.Test.FakeFS;

namespace YoloDev.AspNetCore.Assets.Test
{
  public class FunctionalTest
  {
    [Fact]
    public async Task NoManifestTests()
    {
      var root = new PathString("/root");
      var context = CreateContext(root);
      var filePath = new PathString("/1.js");

      var files = CreateSimpleFileSystemWithNoManifest();
      var provider = new AssetManifestStreamProvider(files);

      var manifestInfo = await provider.FindManifest(filePath);
      Assert.Null(manifestInfo);

      var reader = new AssetManifestReader();
      var cache = new AssetManifestCache(provider, reader);
      var manifest = await cache.GetManifest(filePath);
      Assert.Null(manifest);

      var service = new AssetService(A.Fake<IOptionsSnapshot<AssetOptions>>(), cache);
      var asset = await service.GetAsset(filePath, context);
      Assert.Equal(root.Add(filePath), asset);
    }

    [Fact]
    public async Task WithManifestTests()
    {
      var root = new PathString("/root");
      var context = CreateContext(root);

      var root1Path = new PathString("/1.js");
      var assetsInPath = new PathString("/assets/in.js");
      var assetsInProdPath = new PathString("/assets/in.prod.js");
      var assetsOutPath = new PathString("/assets/out.js");
      var assetsSubInPath = new PathString("/assets/sub/in.js");
      var assetsSubInProdPath = new PathString("/assets/sub/in.prod.js");
      var assetsSubOutPath = new PathString("/assets/sub/out.js");
      var manifestPath = new PathString("/assets/manifest.json");

      var files = CreateSimpleFileSystemWithSingleManifest();
      var provider = new AssetManifestStreamProvider(files);

      var manifestInfo = await provider.FindManifest(root1Path);
      Assert.Null(manifestInfo);
      manifestInfo = await provider.FindManifest(assetsInPath);
      Assert.NotNull(manifestInfo);
      Assert.Equal(manifestPath, manifestInfo.Path);
      manifestInfo = await provider.FindManifest(assetsSubInPath);
      Assert.NotNull(manifestInfo);
      Assert.Equal(manifestPath, manifestInfo.Path);

      var reader = new AssetManifestReader();
      var cache = new AssetManifestCache(provider, reader);
      var manifest = await cache.GetManifest(root1Path);
      Assert.Null(manifest);
      manifest = await cache.GetManifest(assetsInPath);
      Assert.NotNull(manifest);
      Assert.Equal(manifestPath, manifest.Path);
      manifest = await cache.GetManifest(assetsSubInPath);
      Assert.NotNull(manifest);
      Assert.Equal(manifestPath, manifest.Path);

      var service = new AssetService(A.Fake<IOptionsSnapshot<AssetOptions>>(), cache);
      var actual = await service.GetAsset(root1Path, context);
      Assert.Equal(root.Add(root1Path), actual);
      actual = await service.GetAsset(assetsInPath, context);
      Assert.Equal(root.Add(assetsInProdPath), actual);
      actual = await service.GetAsset(assetsSubInPath, context);
      Assert.Equal(root.Add(assetsSubInProdPath), actual);
      actual = await service.GetAsset(assetsOutPath, context);
      Assert.Equal(root.Add(assetsOutPath), actual);
      actual = await service.GetAsset(assetsSubOutPath, context);
      Assert.Equal(root.Add(assetsSubOutPath), actual);
    }

    [Fact]
    public async Task WithNestedManifestsTests()
    {
      var root = new PathString("/root");
      var context = CreateContext(root);

      var root1Path = new PathString("/1.js");
      var assetsInPath = new PathString("/assets/in.js");
      var assetsInProdPath = new PathString("/assets/in.prod.js");
      var assetsOutPath = new PathString("/assets/out.js");
      var assetsSubInPath = new PathString("/assets/sub/in.js");
      var assetsSubInProdPath = new PathString("/assets/sub/in.prod.js");
      var assetsSubOutPath = new PathString("/assets/sub/out.js");
      var manifestPath = new PathString("/assets/manifest.json");
      var subManifestPath = new PathString("/assets/sub/manifest.json");

      var files = CreateSimpleFileSystemWithSingleMultipleManifest();
      var provider = new AssetManifestStreamProvider(files);

      var manifestInfo = await provider.FindManifest(root1Path);
      Assert.Null(manifestInfo);
      manifestInfo = await provider.FindManifest(assetsInPath);
      Assert.NotNull(manifestInfo);
      Assert.Equal(manifestPath, manifestInfo.Path);
      manifestInfo = await provider.FindManifest(assetsSubInPath);
      Assert.NotNull(manifestInfo);
      Assert.Equal(subManifestPath, manifestInfo.Path);

      var reader = new AssetManifestReader();
      var cache = new AssetManifestCache(provider, reader);
      var manifest = await cache.GetManifest(root1Path);
      Assert.Null(manifest);
      manifest = await cache.GetManifest(assetsInPath);
      Assert.NotNull(manifest);
      Assert.Equal(manifestPath, manifest.Path);
      manifest = await cache.GetManifest(assetsSubInPath);
      Assert.NotNull(manifest);
      Assert.Equal(subManifestPath, manifest.Path);

      var service = new AssetService(A.Fake<IOptionsSnapshot<AssetOptions>>(), cache);
      var actual = await service.GetAsset(root1Path, context);
      Assert.Equal(root.Add(root1Path), actual);
      actual = await service.GetAsset(assetsInPath, context);
      Assert.Equal(root.Add(assetsInProdPath), actual);
      actual = await service.GetAsset(assetsSubInPath, context);
      Assert.Equal(root.Add(assetsSubInProdPath), actual);
      actual = await service.GetAsset(assetsOutPath, context);
      Assert.Equal(root.Add(assetsOutPath), actual);
      actual = await service.GetAsset(assetsSubOutPath, context);
      Assert.Equal(root.Add(assetsSubOutPath), actual);
    }

    private static HttpContext CreateContext(PathString basePath)
    {
      var context = A.Fake<HttpContext>();
      var request = A.Fake<HttpRequest>();

      A.CallTo(() => request.PathBase).Returns(basePath);
      A.CallTo(() => context.Request).Returns(request);

      return context;
    }

    private static IFileProvider CreateSimpleFileSystemWithNoManifest()
    {
      return FakeProvider.Create(
        FakeEntry.File("1.js", "const file = 1;"),
        FakeEntry.File("2.js", "const file = 2;")
      );
    }

    private static IFileProvider CreateSimpleFileSystemWithSingleManifest()
    {
      return FakeProvider.Create(
        FakeEntry.File("1.js", "const file = 1;"),
        FakeEntry.File("2.js", "const file = 2;"),
        FakeEntry.Dir("assets",
          FakeEntry.File("manifest.json", "{\"in.js\":\"in.prod.js\",\"sub/in.js\":\"sub/in.prod.js\"}"),
          FakeEntry.File("in.js", "const file = 'in';"),
          FakeEntry.File("in.prod.js", "const file = 'in.prod';"),
          FakeEntry.File("out.js", "const file = 'out';"),
          FakeEntry.Dir("sub",
            FakeEntry.File("in.js", "const file = 'sub/in';"),
            FakeEntry.File("in.prod.js", "const file = 'sub/in.prod';"),
            FakeEntry.File("out.js", "const file = 'sub/out';")
          )
        )
      );
    }

    private static IFileProvider CreateSimpleFileSystemWithSingleMultipleManifest()
    {
      return FakeProvider.Create(
        FakeEntry.File("1.js", "const file = 1;"),
        FakeEntry.File("2.js", "const file = 2;"),
        FakeEntry.Dir("assets",
          FakeEntry.File("manifest.json", "{\"in.js\":\"in.prod.js\"}"),
          FakeEntry.File("in.js", "const file = 'in';"),
          FakeEntry.File("in.prod.js", "const file = 'in.prod';"),
          FakeEntry.File("out.js", "const file = 'out';"),
          FakeEntry.Dir("sub",
            FakeEntry.File("manifest.json", "{\"in.js\":\"in.prod.js\"}"),
            FakeEntry.File("in.js", "const file = 'sub/in';"),
            FakeEntry.File("in.prod.js", "const file = 'sub/in.prod';"),
            FakeEntry.File("out.js", "const file = 'sub/out';")
          )
        )
      );
    }
  }
}
