using System;
using System.Collections.Immutable;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace YoloDev.AspNetCore.Assets.Test
{
  public class AssetManifestTest
  {
    private static readonly PathString DefaultPath = new PathString("/assets");
    private static readonly PathString AssetDevPath = new PathString("/assets/1.js");
    private static readonly PathString AssetProdPath = new PathString("/assets/1.min.js");
    private static readonly ImmutableDictionary<PathString, PathString> EmptyDict =
      ImmutableDictionary.Create<PathString, PathString>();
    private static readonly ImmutableDictionary<PathString, PathString> AssetDict =
      EmptyDict.Add(AssetDevPath, AssetProdPath);

    [Fact]
    public void ConstructorThrowsIfDirIsNull()
    {
      var exn = Assert.Throws<ArgumentNullException>("path", () => new AssetManifest(null, EmptyDict));
    }

    [Fact]
    public void ConstructorThrowsIfDictIsNull()
    {
      var exn = Assert.Throws<ArgumentNullException>("assets", () => new AssetManifest(DefaultPath, null));
    }

    [Fact]
    public void DirReturnsDirInput()
    {
      var sut = new AssetManifest(DefaultPath, EmptyDict);
      Assert.Equal(DefaultPath, sut.Path);
    }

    [Fact]
    public void ResolveReturnsInputIfAssetIsNotFound()
    {
      var sut = new AssetManifest(DefaultPath, EmptyDict);
      var result = sut.Resolve(AssetDevPath);
      Assert.Equal(AssetDevPath, result);
    }

    [Fact]
    public void ResolveReturnsAssetPathIfFound()
    {
      var sut = new AssetManifest(DefaultPath, AssetDict);
      var result = sut.Resolve(AssetDevPath);
      Assert.Equal(AssetProdPath, result);
    }
  }
}
